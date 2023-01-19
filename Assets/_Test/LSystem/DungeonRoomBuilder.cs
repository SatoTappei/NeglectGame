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
    // �C���X�y�N�^�[�œo�^���Ă��镔���̐��̍��v
    readonly int RoomEntranceDicCap = 16;
    // �����̑傫�����ő�5*5�Ȃ̂�
    readonly int RoomRangeSetCap = 25;

    [Header("�������镔���̃f�[�^")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    /// <summary>�����̏o������̐��ʂ̒ʘH�̌����ڂ��C������̂ŕێ����Ă���</summary>
    Dictionary<Vector3Int, Direction> _roomEntranceDic;
    /// <summary>���x�������͈̔͂��i�[����̂Ń����o�ϐ��Ƃ��ĕێ����Ă���</summary>
    HashSet<Vector3Int> _roomRangeSet;

    internal IReadOnlyDictionary<Vector3Int, Direction> GetRoomEntranceDataAll() => _roomEntranceDic;

    void Awake()
    {
        _helper = new();
        _roomEntranceDic = new Dictionary<Vector3Int, Direction>(RoomEntranceDicCap);
        _roomRangeSet = new HashSet<Vector3Int>(RoomRangeSetCap);
    }

    /// <summary>�ʘH�̎��͂ɕ��������Ă�̂ŁA��ɒʘH�����ĂĂ���K�v������</summary>
    internal void BuildDungeonRoom(IReadOnlyDictionary<Vector3Int, DungeonPassMassData> massDataAll)
    {
        // ���������Ă���Ƃ��ĒʘH�̑��̍��W�������^�Ŏ󂯎��
        IReadOnlyCollection<Vector3Int> massPosAll = massDataAll.Keys as IReadOnlyCollection<Vector3Int>;
        Dictionary<Vector3Int, Direction> estimatePosDic = GetAvailablePosDic(massPosAll);
        HashSet <Vector3Int> alreadyBuildPosSet = new HashSet<Vector3Int>(massPosAll);

        int roomIndex = 0;
        foreach (KeyValuePair<Vector3Int, Direction> pair in estimatePosDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            Vector3Int pos = pair.Key;
            Direction dir = pair.Value;
            DungeonRoomData data = _roomDataArr[roomIndex];
            HashSet<Vector3Int> roomRangeSet = GetRoomRangeSet(pos, dir, data.Width, data.Depth);

            if (!IsAvailableRange(roomRangeSet, alreadyBuildPosSet)) continue;

            if (data.IsAvailable())
            {
                Instantiate(data.GetPrefab(), pos, _helper.ConvertToInverseRot(dir), _parent);
                // �������m���d�Ȃ�Ȃ��悤�ɐ������������̍��W���R���N�V�����Ɋi�[���Ă���
                foreach (Vector3Int v in roomRangeSet)
                    alreadyBuildPosSet.Add(v);

                _roomEntranceDic.Add(pos, dir);
            }
            else
            {
                // �������镔����������΃��[�v�𔲂���
                if (++roomIndex > _roomDataArr.Length - 1) break;
            }
        }
    }

    /// <summary>�ʘH�̑��̍��W�𒲂ׂāA���������Ă�����W�̎����^�Ƃ��ĕԂ�</summary>
    Dictionary<Vector3Int, Direction> GetAvailablePosDic(IReadOnlyCollection<Vector3Int> massPosAll)
    {
        // �����e�ʂ͒ʘH�̗��e����z�肵�ĒʘH�̐�*2��p�ӂ��Ă���
        Dictionary<Vector3Int, Direction> dic = new Dictionary<Vector3Int, Direction>(massPosAll.Count * 2);

        foreach (Vector3Int pos in massPosAll)
        {
            (int dirs, _) = _helper.GetNeighbourBinary(pos, massPosAll);

            // �e�����ɒʘH��������΂��̕����𕔉��𐶐��\�ȏꏊ�Ƃ��Ď����ɒǉ�����
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryForward)) Add(Direction.Forward);
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryBack))    Add(Direction.Back);
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryLeft))    Add(Direction.Left);
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryRight))   Add(Direction.Right);

            void Add(Direction dir)
            {
                Vector3Int placePos = pos + _helper.ConvertToPos(dir);
                // �d���`�F�b�N
                if (dic.ContainsKey(placePos)) return;
                dic.Add(placePos, dir);
            }
        }

        return dic;
    }

    /// <summary>�������镔���͈̔͂��擾����</summary>
    HashSet<Vector3Int> GetRoomRangeSet(Vector3Int pos, Direction dir, int width, int depth)
    {
        // �J��Ԃ��Ăяo�����\�b�h�Ȃ̂ŉ����new�����ɋ�ɂ��Ďg���܂킷
        _roomRangeSet.Clear();

        for (int i = 0; i < depth; i++)
        {
            Vector3Int center = pos;
            switch (dir)
            {
                case Direction.Forward:
                    center.z += i * _helper.PrefabScale;
                    Add(center, Vector3Int.right);
                    break;
                case Direction.Back:
                    center.z -= i * _helper.PrefabScale;
                    Add(center, Vector3Int.right);
                    break;
                case Direction.Left:
                    center.x -= i * _helper.PrefabScale;
                    Add(center, Vector3Int.forward);
                    break;
                case Direction.Right:
                    center.x += i * _helper.PrefabScale;
                    Add(center, Vector3Int.forward);
                    break;
            }
        }

        return _roomRangeSet;

        // �w�肳�ꂽ���W�Ƃ��̏㉺�������͍��E�̍��W��ǉ����Ă���
        void Add(Vector3Int center, Vector3Int dir)
        {
            // ���_��ǉ�
            _roomRangeSet.Add(center);
            // ���E�̕�����ǉ�
            for (int i = 1; i <= width / 2; i++)
            {
                _roomRangeSet.Add(center + dir * i * _helper.PrefabScale);
                _roomRangeSet.Add(center - dir * i * _helper.PrefabScale);
            }
        }
    }

    bool IsAvailableRange(IReadOnlyCollection<Vector3Int> roomRangeSet, 
                          IReadOnlyCollection<Vector3Int> alreadyBuildPosSet)
    {
        foreach (Vector3Int pos in roomRangeSet)
        {
            if (alreadyBuildPosSet.Contains(pos)) 
                return false;
        }

        return true;
    }
}
