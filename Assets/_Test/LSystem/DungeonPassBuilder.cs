using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonBuildingHelper.Direction;

/// <summary>
/// ������ɑΉ������_���W�����̒ʘH�����Ă�R���|�[�l���g
/// </summary>
public class DungeonPassBuilder : MonoBehaviour
{
    enum TurtleCommand
    {
        Forward = 'F',
        RotRight = '+',
        RotLeft = '-',
        Save = '[',
        Load = ']',
    }

    readonly int PrefabScale = 3;
    readonly int DecreaseDist = 2;
    readonly int SaveStackCap = 4;
    readonly int PassDicCap = 64;
    readonly int EdgePassSetCap = 16;

    readonly int bForward = 0b1000;
    readonly int bBack = 0b0100;
    readonly int bLeft = 0b0010;
    readonly int bRight = 0b0001;

    // TODO:���݂͉��̂��߂�����LSystem�̎Q�Ƃ��������Ă��邪�A��X�ړ������邱�Ƃ𗯈ӂ��Ă���
    [SerializeField] LSystem _lSystem;
    [Header("�_���W�����̒ʘH���\�����镔�i")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("�����������i�̐e")]
    [SerializeField] Transform _parent;

    //List<Vector3Int> _posList = new List<Vector3Int>(10);
    Dictionary<Vector3Int, GameObject> _passDic;
    /// <summary>�e�ʘH�̗��[��ێ����Ă���</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Start()
    {
        //_dist = MaxDist;
        _passDic = new Dictionary<Vector3Int, GameObject>(PassDicCap);
        _fixPassSet = new HashSet<Vector3Int>(EdgePassSetCap);
        ConvertToGameObject(_lSystem.Generate());
    }

    /// <summary>��������_���W�����̕��i�v���n�u�ɕϊ�����</summary>
    void ConvertToGameObject(string str)
    {
        // �Z�[�u/���[�h�̃R�}���h�p
        Stack<TurtleParam> saveStack = new Stack<TurtleParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        //Vector3Int tempPos = Vector3Int.zero;
        Vector3Int dir = Vector3Int.forward;
        int dist = 8;

        //_posList.Add(currentPos);
        
        foreach (char command in str)
        {
            switch ((TurtleCommand)command)
            {
                case TurtleCommand.Forward:
                    GeneratePass(currentPos, dir, dist);
                    currentPos = currentPos + dir * dist * PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                case TurtleCommand.RotRight:
                    dir = RotDir(dir, isPositive: true);
                    break;
                case TurtleCommand.RotLeft:
                    dir = RotDir(dir, isPositive: false);
                    break;
                case TurtleCommand.Save:
                    saveStack.Push(new TurtleParam(currentPos, dir, dist));
                    //Debug.Log($"Push:{currentPos},{dir},{dist}");
                    break;
                case TurtleCommand.Load:
                    if (saveStack.Count == 0) break;
                    TurtleParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dir        = param.Dir;
                    dist       = param.Dist;
                    //Debug.Log($"Pop:{currentPos},{dir},{dist}");
                    break;
            }
        }

        FixPass();
    }

    /// <summary>�����̒ʘH�𐶐�����</summary>
    void GeneratePass(Vector3Int startPos, Vector3Int dir, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Vector3Int pos = startPos + dir * i * PrefabScale;
            // �������W�ɐ������Ȃ��悤�Ƀ`�F�b�N
            if (_passDic.ContainsKey(pos)) continue;

            Quaternion rot = Quaternion.identity;
            if (dir == Vector3Int.right || dir == Vector3Int.left) rot = Quaternion.Euler(0, 90, 0);

            GameObject go = Instantiate(_passPrefab, pos, rot, _parent);
            // ���������ʘH��M�邽�߂Ɏ����ɒǉ����Ă���
            _passDic.Add(pos, go);

            bool require = i / 2 == 1;
            // �����ōi�����}�X�Ǝn�_�ƏI�_���p�̃R���N�V�����ɒǉ�����
            // �����������Ɛ��x���オ�邪�������ׂ����ˏオ��
            if (require || i == 0 || i == dist - 1)
                _fixPassSet.Add(pos);
        }
    }

    /// <summary>�ʘH����a���̂Ȃ������ڂɏC������</summary>
    void FixPass()
    {
        DungeonPassHelper helper = new DungeonPassHelper();
        foreach(Vector3Int pos in _fixPassSet)
        {
            // ���̍��W���O�㍶�E�ǂ̕����ɐڑ�����Ă��邩�A�����ڑ�����Ă��邩
            (int dirs, int count) = helper.GetNeighbourInt(pos, _passDic.Keys);
            bool dirForward = (dirs & bForward) == bForward;
            bool dirBack =    (dirs & bBack)    == bBack;
            bool dirLeft =    (dirs & bLeft)    == bLeft;
            bool dirRight =   (dirs & bRight)   == bRight;

            Quaternion rot = Quaternion.identity;
            switch (count)
            {
                // �s���~�܂�
                case 1:
                    if      (dirBack)  rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirRight) rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirLeft)  rot.eulerAngles = new Vector3(0, -90, 0);

                    Instantiate(_passEndPrefab, pos, rot);
                    break;
                // �p
                case 2:
                    // �㉺�������͍��E�ɐڑ�����Ă���ꍇ�͒ʘH�Ȃ̂ŉ������Ȃ�
                    if ((dirForward && dirBack) || (dirRight && dirLeft)) 
                        continue;

                    if      (dirForward && dirRight) rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirLeft && dirForward)  rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirRight && dirBack)    rot.eulerAngles = new Vector3(0, -90, 0);

                    Instantiate(_cornerPrefab, pos, rot);
                    break;
                // �����H
                case 3:
                    if      (dirForward && dirBack && dirLeft)  rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirBack && dirRight && dirLeft)    rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirForward && dirRight && dirLeft) rot.eulerAngles = new Vector3(0, -90, 0);

                    Instantiate(_tJunctionPrefab, pos, rot);
                    break;
                // �\���H
                case 4:
                    Instantiate(_crossPrefab, pos, rot);
                    break;
            }

            // �u��������̂Ō��������I�u�W�F�N�g�͍폜����
            Destroy(_passDic[pos]);
        }
    }

    /// <summary>�O�㍶�E�̕�������]������</summary>
    /// <param name="isPositive">true���ƑO�E�㍶�̎��v���Afalse���Ɣ����v���</param>
    Vector3Int RotDir(Vector3Int currentDir, bool isPositive)
    {
        if (currentDir == Vector3Int.forward)
        {
            if (isPositive) return Vector3Int.right;
            else            return Vector3Int.left;
        }
        else if (currentDir == Vector3Int.right)
        {
            if (isPositive) return Vector3Int.back;
            else            return Vector3Int.forward;
        }
        else if (currentDir == Vector3Int.back)
        {
            if (isPositive) return Vector3Int.left;
            else            return Vector3Int.right;
        }
        else if (currentDir == Vector3Int.left)
        {
            if (isPositive) return Vector3Int.forward;
            else            return Vector3Int.back;
        }
        else
        {
            Debug.LogError("�㉺���E�ȊO�̊p�x�ł��B: " + currentDir);
            return Vector3Int.zero;
        }
    }
}
