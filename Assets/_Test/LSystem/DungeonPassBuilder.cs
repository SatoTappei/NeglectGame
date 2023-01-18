using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;
using ComponentShape = DungeonComponentData.ComponentShape;

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
    // �킩��₷���悤�ɂ��邽�߂̃e�X�g�p�̃v���n�u
    [SerializeField] GameObject _test;

    DungeonHelper _helper;
    Dictionary<Vector3Int, DungeonComponentData> _passMassDic;
    /// <summary>������Ɍ����ڂ��C�����邽�߂ɏ����𖞂������ʘH��ێ����Ă���</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new DungeonHelper();
        _passMassDic = new Dictionary<Vector3Int, DungeonComponentData>(PassDicCap);
        _fixPassSet = new HashSet<Vector3Int>(EdgePassSetCap);
    }

    internal IReadOnlyDictionary<Vector3Int, DungeonComponentData> GetMassDataAll() => _passMassDic;

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

            Quaternion rot = Quaternion.identity;
            if      (dirVec == Vector3Int.right) rot = Quaternion.Euler(0, 90, 0);
            else if (dirVec == Vector3Int.left)  rot = Quaternion.Euler(0, -90, 0);

            GameObject go = Instantiate(_passPrefab, pos, rot, _parent);
            Direction dir = DungeonComponentData.ConvertToDir(dirVec);
            DungeonComponentData massData = new DungeonComponentData(pos, dir, ComponentShape.Pass, go, 2);
            
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
            (int dirs, int count) = _helper.GetNeighbourInt(pos, _passMassDic.Keys);
            bool dirForward = (dirs & _helper.BForward) == _helper.BForward;
            bool dirBack =    (dirs & _helper.BBack)    == _helper.BBack;
            bool dirLeft =    (dirs & _helper.BLeft)    == _helper.BLeft;
            bool dirRight =   (dirs & _helper.BRight)   == _helper.BRight;

            //Quaternion rot = Quaternion.identity;
            float rotY = 0;
            GameObject go = null;
            ComponentShape shape = ComponentShape.Pass;
            switch (count)
            {
                // �s���~�܂�
                case 1:
                    if      (dirForward)  rotY = 180;
                    else if (dirRight) rotY = -90;
                    else if (dirLeft)  rotY = 90;

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
                    else if (/*dirBack*/dirForward && dirRight && dirLeft)    rotY = 180;
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

            Quaternion rot = Quaternion.Euler(0, rotY, 0);

            // �u��������̂Ō��������I�u�W�F�N�g�͍폜����
            Destroy(_passMassDic[pos].Obj);
            _passMassDic[pos].Dir = DungeonComponentData.ConvertToDir(rotY);
            _passMassDic[pos].Shape = shape;
            _passMassDic[pos].Obj = Instantiate(go, pos, rot, _parent);
            _passMassDic[pos].Connect = count;
        }
    }

    /// <summary>�����̏o�����̐��ʂ̃}�X�𑀍삷��̂Ő�ɕ����𐶐����Ă����K�v������</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataAll)
    {
        // �ʘH�ɑ΂��ĕ������ڑ������ƒʘH�̐ڑ�����+1�����
        // ����ʘH�ɑ΂��Ĕ��Α�������������ڑ������ꍇ������
        // TODO:�����^��pair����U�ϐ��ɑ�����Č��₷�����Ă��珈������
        foreach (KeyValuePair<Vector3Int, Direction> pair in roomEntranceDataAll)
        {
            Vector3Int pos = pair.Key;
            Direction dir = pair.Value;
            // �o�����̍��W�ƕ����������Ă���������畔���̐��ʂ̍��W�����߂�
            Vector3Int frontPos = pos - ConvertToVec3(dir);
            DungeonComponentData frontmassData = _passMassDic[frontPos];

            frontmassData.Connect++;

            switch (frontmassData.Connect)
            {
                case 4:
                    Destroy(frontmassData.Obj);
                    frontmassData.Obj = Instantiate(_crossPrefab, frontPos, Quaternion.identity);
                    break;
                case 3:
                    Destroy(frontmassData.Obj);

                    // �אڃ}�X�̏��ƕ������ǂ̕����Ɍ����Ă��邩���킩���Ă���
                    Quaternion rot3 = Quaternion.identity;
                    // �ʘH�ɕ������אڂ��Đ��������p�^�[��
                    if (frontmassData.Shape == ComponentShape.Pass)
                    {
                        if (dir == Direction.Forward) rot3.eulerAngles = new Vector3(0, 180, 0);
                        if (dir == Direction.Back) rot3.eulerAngles = new Vector3(0, 0, 0);
                        if (dir == Direction.Left) rot3.eulerAngles = new Vector3(0, 90, 0);
                        if (dir == Direction.Right) rot3.eulerAngles = new Vector3(0, -90, 0);
                    }
                    // �ʘH�̒[�ŕ���2�����ݍ��ރp�^�[��
                    if (frontmassData.Shape == ComponentShape.PassEnd)
                    {
                        if(frontmassData.Dir == Direction.Right)
                        {
                            rot3.eulerAngles = new Vector3(0, 90, 0);
                        }
                        else if(frontmassData.Dir == Direction.Left)
                        {
                            rot3.eulerAngles = new Vector3(0, -90, 0);
                        }
                        else if (frontmassData.Dir == Direction.Forward)
                        {
                            rot3.eulerAngles = new Vector3(0, 0, 0);
                        }
                        else if (frontmassData.Dir == Direction.Back)
                        {
                            rot3.eulerAngles = new Vector3(0, 180, 0);
                        }
                    }
                    // �ʘH�̊p�ɕ��������������p�^�[��
                    if (frontmassData.Shape == ComponentShape.Corner)
                    {
                        if (dir == Direction.Forward)
                        {
                            if (frontmassData.Dir == Direction.Forward)
                            {
                                rot3.eulerAngles = new Vector3(0, 90, 0);
                            }
                            else if (frontmassData.Dir == Direction.Left)
                            {
                                rot3.eulerAngles = new Vector3(0, -90, 0);
                            }
                        }
                        else if(dir == Direction.Back)
                        {
                            if (frontmassData.Dir == Direction.Back)
                            {
                                rot3.eulerAngles = new Vector3(0, -90, 0);
                                
                            }
                            else if (frontmassData.Dir == Direction.Right)
                            {
                                rot3.eulerAngles = new Vector3(0, 90, 0);
                            }
                        }
                        else if (dir == Direction.Left)
                        {
                            if (frontmassData.Dir == Direction.Back)
                            {
                                rot3.eulerAngles = new Vector3(0, -180, 0);
                                Debug.Log("������");
                            }
                        }
                        else if (dir == Direction.Right)
                        {
                            if (frontmassData.Dir == Direction.Right)
                            {
                                rot3.eulerAngles = new Vector3(0, 180, 0);
                                Debug.Log("�݂�");
                            }
                        }
                    }

                    frontmassData.Obj = Instantiate(_tJunctionPrefab, frontPos, rot3);
                    break;
                case 2:
                    Destroy(frontmassData.Obj);

                    Quaternion rot = Quaternion.identity;

                    if ((dir == Direction.Forward && frontmassData.Dir == Direction.Forward) ||
                        (dir == Direction.Back && frontmassData.Dir == Direction.Back) ||
                        (dir == Direction.Left && frontmassData.Dir == Direction.Left) ||
                        (dir == Direction.Right && frontmassData.Dir == Direction.Right))
                    {


                        if (dir == Direction.Left || dir == Direction.Right)
                        {
                            rot.eulerAngles = new Vector3(0, 90, 0);
                        }

                        frontmassData.Obj = Instantiate(_passPrefab, frontPos, rot);
                    }
                    else
                    {
                        Quaternion rot2 = Quaternion.identity;

                        if (dir == Direction.Forward)
                        {
                            if (frontmassData.Dir == Direction.Forward)
                            {
                                rot2.eulerAngles = new Vector3(0, 11, 0);
                            }
                            else if (frontmassData.Dir == Direction.Left)
                            {
                                rot2.eulerAngles = new Vector3(0, 180, 0);
                            }
                            if (frontmassData.Dir == Direction.Back)
                            {
                                rot2.eulerAngles = new Vector3(0, 33, 0);

                            }
                            else if (frontmassData.Dir == Direction.Right)
                            {
                                rot2.eulerAngles = new Vector3(0, 90, 0);
                            }
                        }
                        else if (dir == Direction.Back)
                        {
                            if (frontmassData.Dir == Direction.Forward)
                            {
                                rot2.eulerAngles = new Vector3(0, 11, 0);
                            }
                            else if (frontmassData.Dir == Direction.Left)
                            {
                                rot2.eulerAngles = new Vector3(0, -90, 0);
                            }
                            if (frontmassData.Dir == Direction.Back)
                            {
                                rot2.eulerAngles = new Vector3(0, 33, 0);

                            }
                            else if (frontmassData.Dir == Direction.Right)
                            {
                                rot2.eulerAngles = new Vector3(0, 0, 0);
                            }
                        }
                        else if (dir == Direction.Left)
                        {
                            if (frontmassData.Dir == Direction.Forward)
                            {
                                rot2.eulerAngles = new Vector3(0, 0, 0);
                            }
                            else if (frontmassData.Dir == Direction.Left)
                            {
                                rot2.eulerAngles = new Vector3(0, 22, 0);
                            }
                            if (frontmassData.Dir == Direction.Back)
                            {
                                rot2.eulerAngles = new Vector3(0, 90, 0);

                            }
                            else if (frontmassData.Dir == Direction.Right)
                            {
                                rot2.eulerAngles = new Vector3(0, 44, 0);
                            }
                        }
                        else if (dir == Direction.Right)
                        {
                            if (frontmassData.Dir == Direction.Forward)
                            {
                                rot2.eulerAngles = new Vector3(0, -90, 0);
                            }
                            else if (frontmassData.Dir == Direction.Left)
                            {
                                rot2.eulerAngles = new Vector3(0, 22, 0);
                            }
                            if (frontmassData.Dir == Direction.Back)
                            {
                                rot2.eulerAngles = new Vector3(0, 180, 0);

                            }
                            else if (frontmassData.Dir == Direction.Right)
                            {
                                rot2.eulerAngles = new Vector3(0, 44, 0);
                            }
                        }

                        frontmassData.Obj = Instantiate(_cornerPrefab, frontPos, rot2);
                    }
                    break;
                case 1:
                    Destroy(frontmassData.Obj);
                    frontmassData.Obj = Instantiate(_passEndPrefab, frontPos, Quaternion.identity);
                    break;
            }
        }
    }

    // TODO:DungeonRoomBuilder�ɂ��������\�b�h������̂Ń��t�@�N�^�����O����
    Vector3Int ConvertToVec3(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Vector3Int.forward * _helper.PrefabScale;
            case Direction.Back:
                return Vector3Int.back * _helper.PrefabScale;
            case Direction.Left:
                return Vector3Int.left * _helper.PrefabScale;
            case Direction.Right:
                return Vector3Int.right * _helper.PrefabScale;
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł��B: " + dir);
                return Vector3Int.zero;
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
