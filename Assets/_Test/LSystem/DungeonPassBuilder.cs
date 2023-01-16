using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

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
    //[SerializeField] GameObject _test;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _passDic;
    /// <summary������Ɍ����ڂ��C�����邽�߂ɏ����𖞂������ʘH��ێ����Ă���</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new DungeonHelper();
        _passDic = new Dictionary<Vector3Int, GameObject>(PassDicCap);
        _fixPassSet = new HashSet<Vector3Int>(EdgePassSetCap);
    }

    /// <summary>�S�Ă̐��������ʘH�̃}�X�̍��W���擾����</summary>
    internal IReadOnlyCollection<Vector3Int> GetPassPosAll() => _passDic.Keys;

    /// <summary>��������_���W�����̕��i�v���n�u�ɕϊ�����</summary>
    internal void ConvertToGameObject(string str)
    {
        // �Z�[�u/���[�h�̃R�}���h�p
        Stack<TurtleParam> saveStack = new Stack<TurtleParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int dir = Vector3Int.forward;
        int dist = MaxPassDist;
        
        foreach (char command in str)
        {
            switch ((TurtleCommand)command)
            {
                case TurtleCommand.Forward:
                    GeneratePass(currentPos, dir, dist);
                    currentPos = currentPos + dir * dist * _helper.PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                case TurtleCommand.RotRight:
                    dir = Rotate(dir, isPositive: true);
                    break;
                case TurtleCommand.RotLeft:
                    dir = Rotate(dir, isPositive: false);
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
            Vector3Int pos = startPos + dir * i * _helper.PrefabScale;
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
        foreach(Vector3Int pos in _fixPassSet)
        {
            // ���̍��W���O�㍶�E�ǂ̕����ɐڑ�����Ă��邩�A�����ڑ�����Ă��邩
            (int dirs, int count) = _helper.GetNeighbourInt(pos, _passDic.Keys);
            bool dirForward = (dirs & _helper.BForward) == _helper.BForward;
            bool dirBack =    (dirs & _helper.BBack)    == _helper.BBack;
            bool dirLeft =    (dirs & _helper.BLeft)    == _helper.BLeft;
            bool dirRight =   (dirs & _helper.BRight)   == _helper.BRight;

            Quaternion rot = Quaternion.identity;
            GameObject go = null;
            switch (count)
            {
                // �s���~�܂�
                case 1:
                    if      (dirBack)  rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirRight) rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirLeft)  rot.eulerAngles = new Vector3(0, -90, 0);

                    go = Instantiate(_passEndPrefab, pos, rot, _parent);
                    break;
                // �p
                case 2:
                    // �㉺�������͍��E�ɐڑ�����Ă���ꍇ�͒ʘH�Ȃ̂ŉ������Ȃ�
                    if ((dirForward && dirBack) || (dirRight && dirLeft)) 
                        continue;

                    if      (dirForward && dirRight) rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirLeft && dirForward)  rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirRight && dirBack)    rot.eulerAngles = new Vector3(0, -90, 0);

                    go = Instantiate(_cornerPrefab, pos, rot, _parent);
                    break;
                // �����H
                case 3:
                    if      (dirForward && dirBack && dirLeft)  rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirBack && dirRight && dirLeft)    rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirForward && dirRight && dirLeft) rot.eulerAngles = new Vector3(0, -90, 0);

                    go = Instantiate(_tJunctionPrefab, pos, rot, _parent);
                    break;
                // �\���H
                case 4:
                    go = Instantiate(_crossPrefab, pos, rot, _parent);
                    break;
            }

            // �u��������̂Ō��������I�u�W�F�N�g�͍폜����
            Destroy(_passDic[pos]);

            // TODO:���}���u�I�ɒǉ����Ă���̂ł悭��������
            _passDic[pos] = go;
        }
    }

    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> dic)
    {
        // �ʘH�ɑ΂��ĕ������ڑ������ƒʘH�̐ڑ�����+1�����
        // ����ʘH�ɑ΂��Ĕ��Α�������������ڑ������ꍇ������
        foreach (var v in dic)
        {
            GameObject go = _passDic[v.Key - GetSidePos(v.Value)];

            // TODO:���O�Ŕ��肵�ď����𕪊򂷂�A�e�X�g�Ȃ̂ŕʂ̕��@����������K�v����
            //      ���Α�����̐ڑ����l������Ă��Ȃ��̂Œ���
            //      ��������]���s��
            switch (go.name)
            {
                case "Dungeon_Pass(Clone)":
                    Instantiate(_tJunctionPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    Destroy(go);
                    break;
                case "Dungeon_PassEnd(Clone)":
                    // ���ʂ̏ꍇ�ƍ��E�ɐ������ꂽ�ꍇ�ŕ��򂵂Ȃ��Ƃ����Ȃ�
                    if ((v.Value == Direction.Forward &&  Hoge(go) == Direction.Back) ||
                        (v.Value == Direction.Back && Hoge(go) == Direction.Forward) ||
                        (v.Value == Direction.Left && Hoge(go) == Direction.Right) ||
                        (v.Value == Direction.Right && Hoge(go) == Direction.Left))
                    {
                        Instantiate(_passPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(_cornerPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    }
                    Destroy(go);
                    break;
                case "Dungeon_Corner(Clone)":
                    Instantiate(_tJunctionPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    Destroy(go);
                    break;
                case "Dungeon_TJunction(Clone)":
                    Instantiate(_crossPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    Destroy(go);
                    break;
            }
            //Instantiate(_test, v.Key - GetSidePos(v.Value), Quaternion.identity);
        }

        Direction Hoge(GameObject go)
        {
            Debug.Log(go.transform.rotation.y);
            if (go.transform.rotation.y == 0)
            {
                return Direction.Forward; 
            }
            if (go.transform.rotation.y == 0.7071068f)
            {
                return Direction.Right;
            }
            if (go.transform.rotation.y == -0.7071068f)
            {
                return Direction.Left;
            }
            if (go.transform.rotation.y == 1)
            {
                return Direction.Back;
            }

            Debug.LogError("�Ȃ񂩕�");
            return Direction.Forward;
        }
    }

    // TODO:DungeonRoomBuilder�ɂ��������\�b�h������̂Ń��t�@�N�^�����O����
    Vector3Int GetSidePos(Direction dir)
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

    /// <summary>�O�㍶�E�̕�������]������</summary>
    /// <param name="isPositive">true���ƑO�E�㍶�̎��v���Afalse���Ɣ����v���</param>
    Vector3Int Rotate(Vector3Int currentDir, bool isPositive)
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
