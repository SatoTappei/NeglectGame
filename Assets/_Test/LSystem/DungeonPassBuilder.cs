using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;
using ComponentShape = DungeonPassMassData.ComponentShape;

/// <summary>
/// ������ɑΉ������_���W�����̒ʘH�����Ă�R���|�[�l���g
/// </summary>
public class DungeonPassBuilder : MonoBehaviour
{
    enum Command
    {
        Forward = 'F',
        RotRight = '+',
        RotLeft = '-',
        Save = '[',
        Load = ']',
    }

    readonly int MaxPassDist = 8;
    readonly int DecreaseDist = 2;
    readonly int SaveStackCap = 4;
    readonly int PassDicCap = 64;
    readonly int EdgePassSetCap = 16;

    [Header("�ʘH���\������v���n�u")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    DungeonPassHelper _pHelper;
    Dictionary<Vector3Int, DungeonPassMassData> _passMassDic;
    /// <summary>������Ɍ����ڂ��C�����邽�߂ɏ����𖞂������ʘH��ێ����Ă���</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new();
        _pHelper = new();
        _passMassDic = new Dictionary<Vector3Int, DungeonPassMassData>(PassDicCap);
        _fixPassSet = new HashSet<Vector3Int>(EdgePassSetCap);
    }

    internal IReadOnlyDictionary<Vector3Int, DungeonPassMassData> GetMassDataAll() => _passMassDic;

    internal void BuildDungeonPass(string str)
    {
        // �Z�[�u/���[�h�̃R�}���h�p
        Stack<CursorParam> saveStack = new Stack<CursorParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int dirVec = Vector3Int.forward;
        int dist = MaxPassDist;
        
        foreach (char command in str)
        {
            switch ((Command)command)
            {
                // �����̒ʘH�𐶐����ĒʘH�̐�Ɋ�_���ڂ�
                case Command.Forward:
                    GeneratePassStraight(currentPos, dirVec, dist);
                    currentPos = currentPos + dirVec * dist * _helper.PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                // ��_���E��90����]������
                case Command.RotRight:
                    dirVec = RotDirVec90(dirVec, isPositive: true);
                    break;
                // ��_������90����]������
                case Command.RotLeft:
                    dirVec = RotDirVec90(dirVec, isPositive: false);
                    break;
                // ���݂̊�_���X�^�b�N�ɐς�
                case Command.Save:
                    saveStack.Push(new CursorParam(currentPos, dirVec, dist));
                    //Debug.Log($"Push:{currentPos},{dir},{dist}");
                    break;
                // �X�^�b�N�����_�����o���Ă��̈ʒu�Ɋ�_��ύX����
                case Command.Load:
                    if (saveStack.Count == 0) break;
                    CursorParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dirVec = param.DirVec;
                    dist = param.Dist;
                    //Debug.Log($"Pop:{currentPos},{dir},{dist}");
                    break;
            }
        }

        FixPassVisual();
    }

    void GeneratePassStraight(Vector3Int startPos, Vector3Int dirVec, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Vector3Int pos = startPos + dirVec * i * _helper.PrefabScale;

            if (_passMassDic.ContainsKey(pos)) continue;

            float rotY = _pHelper.GetPassStraightRotY(dirVec);
            GameObject go = Instantiate(_passPrefab, pos, Quaternion.Euler(0, rotY, 0), _parent);

            Direction dir = DungeonPassMassData.ConvertToDir(dirVec);
            DungeonPassMassData massData = new (pos, dir, ComponentShape.Pass, go, 2);
            
            _passMassDic.Add(pos, massData);

            // ��ڂ̃}�X�Ǝn�_�ƏI�_���p�̃R���N�V�����ɒǉ�����
            // �����������ƌ����ڂ̏C�����̐��x���オ�邪�������ׂ����ˏオ��
            if ((i / 2 == 1) || i == 0 || i == dist - 1)
                _fixPassSet.Add(pos);
        }
    }

    void FixPassVisual()
    {
        foreach(Vector3Int pos in _fixPassSet)
        {
            // ���̍��W���O�㍶�E�ǂ̕����ɐڑ�����Ă��邩�Ō�����ύX
            // �����ڑ�����Ă��邩�őΉ����錩���ڂɕύX����
            (int dirs, int count) = _helper.GetNeighbourBinary(pos, _passMassDic.Keys);
            bool dirForward = (dirs & _helper.BForward) == _helper.BForward;
            bool dirBack =    (dirs & _helper.BBack)    == _helper.BBack;
            bool dirLeft =    (dirs & _helper.BLeft)    == _helper.BLeft;
            bool dirRight =   (dirs & _helper.BRight)   == _helper.BRight;

            float rotY = 0;
            GameObject go = null;
            ComponentShape shape = ComponentShape.Pass;
            switch (count)
            {
                // �s���~�܂�
                case 1:
                    if      (dirForward) rotY = 180;
                    else if (dirRight)   rotY = -90;
                    else if (dirLeft)    rotY = 90;

                    go = _passEndPrefab;
                    shape = ComponentShape.PassEnd;
                    break;
                // �p
                case 2:
                    // �㉺�������͍��E�ɐڑ�����Ă���ꍇ�͒ʘH�Ȃ̂ŉ������Ȃ�
                    if ((dirForward && dirBack) || (dirRight && dirLeft)) 
                        continue;

                    if      (dirForward && dirRight) rotY = 180;
                    else if (dirLeft && dirForward)  rotY = 90;
                    else if (dirRight && dirBack)    rotY = -90;

                    go = _cornerPrefab;
                    shape = ComponentShape.Corner;
                    break;
                // �����H
                case 3:
                    if      (dirForward && dirBack && dirLeft)  rotY = 90;
                    else if (dirForward && dirRight && dirLeft) rotY = 180;
                    else if (dirForward && dirBack && dirRight) rotY = -90;

                    go = _tJunctionPrefab;
                    shape = ComponentShape.TJunction;
                    break;
                // �\���H
                case 4:
                    go = _crossPrefab;
                    shape = ComponentShape.Cross;
                    break;
            }

            // �u��������̂Ō��������I�u�W�F�N�g�͍폜����
            Destroy(_passMassDic[pos].Obj);
            _passMassDic[pos].Dir = DungeonPassMassData.ConvertToDir(rotY);
            _passMassDic[pos].Shape = shape;
            _passMassDic[pos].Obj = Instantiate(go, pos, Quaternion.Euler(0, rotY, 0), _parent);
            _passMassDic[pos].Connect = count;
        }
    }

    /// <summary>�����̏o�����̐��ʂ̃}�X�𑀍삷��̂Ő�ɕ����𐶐����Ă����K�v������</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataAll)
    {
        DungeonPassHelper dungeonCalc = new();

        foreach (KeyValuePair<Vector3Int, Direction> pair in roomEntranceDataAll)
        {
            Vector3Int roomPos = pair.Key;
            Direction roomDir = pair.Value;
            // �o�����̍��W�ƕ����������Ă���������畔���̐��ʂ̍��W�����߂�
            Vector3Int frontPos = roomPos - _helper.ConvertToPos(roomDir);
            DungeonPassMassData frontmassData = _passMassDic[frontPos];

            // �I�u�W�F�N�g��u��������̂ňȑO�̂��̂��폜����
            Destroy(frontmassData.Obj);

            // ���ʂ̃}�X�̐ڑ�����+1���āA�����̏o������ƌq�����������ڂɕύX����
            switch (++frontmassData.Connect)
            {
                // �\���H
                case 4:
                    frontmassData.Obj = Instantiate(_crossPrefab, frontPos, Quaternion.identity, _parent);
                    break;
                // �����H
                case 3:
                    float rot3 = dungeonCalc.GetTJunctionRotY(roomDir, frontmassData.Dir, frontmassData.Shape);
                    frontmassData.Obj = Instantiate(_tJunctionPrefab, frontPos, Quaternion.Euler(0, rot3, 0), _parent);
                    break;
                // �����������͊p
                case 2:
                    if (roomDir == frontmassData.Dir)
                    {
                        float rot2 = dungeonCalc.GetPassStraightRotY(roomDir);
                        frontmassData.Obj = Instantiate(_passPrefab, frontPos, Quaternion.Euler(0, rot2, 0), _parent);
                    }
                    else
                    {
                        float rot2 = dungeonCalc.GetCornerRotY(roomDir, frontmassData.Dir);
                        frontmassData.Obj = Instantiate(_cornerPrefab, frontPos, Quaternion.Euler(0, rot2, 0), _parent);
                    }
                    break;
            }
        }
    }

    /// <param name="isPositive">true���ƑO�E�㍶�̎��v���Afalse���Ɣ����v���</param>
    Vector3Int RotDirVec90(Vector3Int dirVec, bool isPositive)
    {
        if (dirVec == Vector3Int.forward)
        {
            if (isPositive) return Vector3Int.right;
            else            return Vector3Int.left;
        }
        else if (dirVec == Vector3Int.right)
        {
            if (isPositive) return Vector3Int.back;
            else            return Vector3Int.forward;
        }
        else if (dirVec == Vector3Int.back)
        {
            if (isPositive) return Vector3Int.left;
            else            return Vector3Int.right;
        }
        else if (dirVec == Vector3Int.left)
        {
            if (isPositive) return Vector3Int.forward;
            else            return Vector3Int.back;
        }
        else
        {
            Debug.LogError("�㉺���E�ȊO�̊p�x�ł��B: " + dirVec);
            return Vector3Int.zero;
        }
    }
}
