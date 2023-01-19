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
    // 3�񏑂���������ɐݒ�
    readonly int PassMassDicCap = 450;
    readonly int EdgePassSetCap = 100;

    [Header("�ʘH���\������v���n�u")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    DungeonPassDirectionCalculator _directionCalculator;
    DungeonPassBinaryCalculator _binaryCalculator;
    Dictionary<Vector3Int, DungeonPassMassData> _passMassDic;
    /// <summary>������Ɍ����ڂ��C�����邽�߂ɏ����𖞂������ʘH��ێ����Ă���</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new();
        _directionCalculator = new();
        _binaryCalculator = new();
        _passMassDic = new Dictionary<Vector3Int, DungeonPassMassData>(PassMassDicCap);
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
                    dirVec = GetRotate90(dirVec, isPositive: true);
                    break;
                // ��_������90����]������
                case Command.RotLeft:
                    dirVec = GetRotate90(dirVec, isPositive: false);
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

            float rotY = _directionCalculator.GetPassStraightRotY(dirVec);
            GameObject go = Instantiate(_passPrefab, pos, Quaternion.Euler(0, rotY, 0), _parent);

            Direction dir = _helper.ConvertToDir(dirVec);
            DungeonPassMassData massData = new (dir, ComponentShape.Pass, go, 2);
            
            _passMassDic.Add(pos, massData);

            // ��ڂ̃}�X�Ǝn�_�ƏI�_���p�̃R���N�V�����ɒǉ�����
            // �����������ƌ����ڂ̏C�����̐��x���オ�邪�������ׂ����ˏオ��
            if ((i / 2 == 1) || i == 0 || i == dist - 1)
                _fixPassSet.Add(pos);
        }
    }

    /// <summary>�n���ꂽ�����x�N�g������90�x��]�����������x�N�g����Ԃ�</summary>
    /// <param name="isPositive">true���ƑO�E�㍶�̎��v���Afalse���Ɣ����v���</param>
    Vector3Int GetRotate90(Vector3Int dirVec, bool isPositive)
    {
        if      (dirVec == Vector3Int.forward) return isPositive ? Vector3Int.right : Vector3Int.left;
        else if (dirVec == Vector3Int.right)   return isPositive ? Vector3Int.back : Vector3Int.forward;
        else if (dirVec == Vector3Int.back)    return isPositive ? Vector3Int.left : Vector3Int.right;
        else if (dirVec == Vector3Int.left)    return isPositive ? Vector3Int.forward : Vector3Int.back;

        Debug.LogError("�㉺���E�ȊO�̊p�x�ł��B: " + dirVec);
        return Vector3Int.zero;
    }

    void FixPassVisual()
    {
        foreach(Vector3Int pos in _fixPassSet)
        {
            // ���̍��W���O�㍶�E�ǂ̕����ɐڑ�����Ă��邩�Ō�����ύX
            // �����ڑ�����Ă��邩�őΉ����錩���ڂɕύX����
            (int dirs, int count) = _helper.GetNeighbourBinary(pos, _passMassDic.Keys);

            switch (count)
            {
                // �s���~�܂�
                case 1:
                    Replace(_binaryCalculator.GetPassEndRotY(dirs), _passEndPrefab, ComponentShape.PassEnd);
                    break;
                // �p
                case 2 when !_binaryCalculator.IsPassStraight(dirs):
                    Replace(_binaryCalculator.GetCornerRotY(dirs), _cornerPrefab, ComponentShape.Corner);
                    break;
                // �����H
                case 3:
                    Replace(_binaryCalculator.GetTJunctionRotY(dirs), _tJunctionPrefab, ComponentShape.TJunction);
                    break;
                // �\���H
                case 4:
                    Replace(0, _crossPrefab, ComponentShape.Cross);
                    break;
            }

            void Replace(float rotY, GameObject go, ComponentShape shape)
            {
                // �I�u�W�F�N�g��u��������̂ňȑO�̂��̂��폜����
                Destroy(_passMassDic[pos].Obj);

                _passMassDic[pos].Replace(dir:     _helper.ConvertToDir(rotY),
                                          shape:   shape,
                                          obj:     Instantiate(go, pos, Quaternion.Euler(0, rotY, 0), _parent),
                                          connect: count);
            }
        }
    }

    /// <summary>�����̏o�����̐��ʂ̃}�X�𑀍삷��̂Ő�ɕ����𐶐����Ă����K�v������</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataAll)
    {
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
                    Replace(_crossPrefab, 0);
                    break;
                // �����H
                case 3:
                    float rot = _directionCalculator.GetTJunctionRotY(roomDir, frontmassData.Dir, frontmassData.Shape);
                    Replace(_tJunctionPrefab, rot);
                    break;
                // �����������͊p
                case 2:
                    if (roomDir == frontmassData.Dir)
                        Replace(_passPrefab, _directionCalculator.GetPassStraightRotY(roomDir));
                    else
                        Replace(_cornerPrefab, _directionCalculator.GetCornerRotY(roomDir, frontmassData.Dir));
                    break;
            }

            void Replace(GameObject go, float rotY)
            {
                frontmassData.Obj = Instantiate(go, frontPos, Quaternion.Euler(0, rotY, 0), _parent);
            }
        }
    }
}
