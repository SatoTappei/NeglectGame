using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// �_���W�����̒ʘH�ɉ����������𗧂Ă�R���|�[�l���g
/// </summary>
public class DungeonRoomBuilder : MonoBehaviour
{
    readonly int RoomDicCap = 16;
    readonly int PlaceDicCap = 64;
    readonly int BlockPosSetCap = 64;
    readonly int RoomRangeSetCap = 9;

    [Header("�����̃v���n�u")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _parent;

    [SerializeField] GameObject _test;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _roomDic;
    /// <summary>���x�������͈̔͂��i�[����̂Ń����o�ϐ��Ƃ��ĕێ����Ă���</summary>
    HashSet<Vector3Int> _roomRangeSet;
    
    void Awake()
    {
        _helper = new DungeonHelper();
        _roomDic = new Dictionary<Vector3Int, GameObject>(RoomDicCap);
        _roomRangeSet = new HashSet<Vector3Int>(RoomRangeSetCap);
    }

    /// <summary>�����̐������s��</summary>
    internal void GenerateRoom(IReadOnlyCollection<Vector3Int> passAll)
    {
        // �����𐶐��\�ȍ��W�̎������쐬����
        Dictionary<Vector3Int, Direction> placeDic = new Dictionary<Vector3Int, Direction>(PlaceDicCap);
        InsertToDic(placeDic, passAll);

        // �����s�\�ȍ��W��ێ����Ă����n�b�V���Z�b�g
        HashSet<Vector3Int> blockPosSet = new HashSet<Vector3Int>(BlockPosSetCap);
        foreach (Vector3Int pos in passAll)
            blockPosSet.Add(pos);

        // ???�����Ɏg��
        int index = 0;

        // �����_���ɕ��ёւ����ݒu���̎��������ɑ�������
        foreach (KeyValuePair<Vector3Int, Direction> pair in placeDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            // �������镔���̃f�[�^
            // �������镔���͕K���������Ȃ��Ă͂����Ȃ��������D��
            // �c�����Ƃ���������߂��Ă����悤�ɕ����𐶐�����
            DungeonRoomData data = _roomDataArr[index];

            HashSet<Vector3Int> roomRangeSet = GetRoomRangeSet(pair.Key, pair.Value, data.Width, data.Depth);

            if (IsAvailable(roomRangeSet, blockPosSet))
            {
                Quaternion rot = GetInverseRot(pair.Value);
                if (data.IsAvailable())
                {
                    Instantiate(data.GetPrefab(), pair.Key, rot, _parent);
                    // �������m���d�Ȃ�Ȃ��悤�ɐ������������̍��W���R���N�V�����Ɋi�[���Ă���
                    foreach (Vector3Int pos in roomRangeSet)
                        blockPosSet.Add(pos);
                }
                else
                {
                    index++;
                }
            }
        }
    }

    /// <summary>�͈͓��Ɋ��ɕ������������`�F�b�N</summary>
    bool IsAvailable(IReadOnlyCollection<Vector3Int> roomRangeSet, IReadOnlyCollection<Vector3Int> blockPosSet)
    {
        foreach (Vector3Int pos in roomRangeSet)
        {
            if (blockPosSet.Contains(pos)) 
                return false;
        }

        return true;
    }

    /// <summary>�������镔���͈̔͂��擾����</summary>
    HashSet<Vector3Int> GetRoomRangeSet(Vector3Int pos, Direction dir, int width, int depth)
    {
        _roomRangeSet.Clear();

        for(int i = 0; i < depth; i++)
        {
            Vector3Int center = pos;
            switch (dir)
            {
                case Direction.Forward:
                    center.z += i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.left, Vector3Int.right);
                    break;
                case Direction.Back:
                    center.z -= i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.left, Vector3Int.right);
                    break;
                case Direction.Left:
                    center.x -= i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.forward, Vector3Int.back);
                    break;
                case Direction.Right:
                    center.x += i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.forward, Vector3Int.back);
                    break;
            }
        }

        return _roomRangeSet;

        // �w�肳�ꂽ���W�Ƃ��̏㉺�������͍��E�̍��W��ǉ����Ă���
        void AddSet(Vector3Int center, Vector3Int dir1, Vector3Int dir2)
        {
            // ���_��ǉ�
            _roomRangeSet.Add(center);
            // ���E�̕�����ǉ�
            for (int i = 1; i <= width / 2; i++)
            {
                Vector3Int side1 = center + dir1 * i * _helper.PrefabScale;
                Vector3Int side2 = center + dir2 * i * _helper.PrefabScale;

                _roomRangeSet.Add(side1);
                _roomRangeSet.Add(side2);
            }
        }
    }

    /// <summary>�����������\�ȏꏊ�������ɑ}������</summary>
    void InsertToDic(Dictionary<Vector3Int, Direction> dic, IReadOnlyCollection<Vector3Int> passAll)
    {
        foreach (Vector3Int pos in passAll)
        {
            // ���̖͂��܂��Ă���}�X�̕��p���擾����
            (int dirs, _) = _helper.GetNeighbourInt(pos, passAll);

            // �e�����ɒʘH��������΂��̕����𕔉��𐶐��\�ȏꏊ�Ƃ��Ď����ɒǉ�����
            if ((dirs & _helper.BForward) != _helper.BForward) AddDic(Direction.Forward);
            if ((dirs & _helper.BBack) != _helper.BBack) AddDic(Direction.Back);
            if ((dirs & _helper.BLeft) != _helper.BLeft) AddDic(Direction.Left);
            if ((dirs & _helper.BRight) != _helper.BRight) AddDic(Direction.Right);

            void AddDic(Direction dir)
            {
                Vector3Int placePos = pos + GetSidePos(dir);
                // �d���`�F�b�N
                if (dic.ContainsKey(placePos)) return;
                dic.Add(placePos, dir);
            }
        }
    }

    /// <summary>�����Ƌt�̉�]���擾����</summary>
    Quaternion GetInverseRot(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Back:
                return Quaternion.Euler(0, 180, 0);
            case Direction.Left:
                return Quaternion.Euler(0, -90, 0);
            case Direction.Right:
                return Quaternion.Euler(0, 90, 0);
            default:
                Debug.LogError("�񋓌^Direction�Œ�`����Ă��Ȃ��l�ł��B: " + dir);
                return Quaternion.identity;
        }
    }

    /// <summary>�����ɉ�����1�}�X�e�̍��W��Ԃ�</summary>
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
}
