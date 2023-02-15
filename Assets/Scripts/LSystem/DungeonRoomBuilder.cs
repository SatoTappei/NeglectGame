using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���W�����̒ʘH�ɉ����������𗧂Ă�R���|�[�l���g
/// </summary>
public class DungeonRoomBuilder : MonoBehaviour
{
    /// <summary>�C���X�y�N�^�[�Ŋ��蓖�Ă��������镔���̐��̍��v�������e�ʂƂ��Ċm�ۂ���</summary>
    static readonly int RoomEntranceDicCap = 16;
    /// <summary>�������镔���̍ő�̑傫���ł���5*5�������e�ʂƂ��Ċm�ۂ���</summary>
    static readonly int RoomRangeListCap = 25;

    [Header("�������镔���̃f�[�^")]
    [SerializeField] DungeonRoomData[] _roomDatas;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _prefabParent;

    DungeonHelper _helper = new();
    /// <summary>�����̏o������̐��ʂ̒ʘH�̌����ڂ��C������̂ŕێ����Ă���</summary>
    Dictionary<Vector3Int, Direction> _roomEntranceDic = new (RoomEntranceDicCap);
    /// <summary>���x�������͈̔͂��i�[����̂Ń����o�ϐ��Ƃ��ĕێ����Ă���</summary>
    List<Vector3Int> _roomRangeList = new (RoomRangeListCap);

    internal IReadOnlyDictionary<Vector3Int, Direction> RoomEntranceDic => _roomEntranceDic;

    /// <summary>�ʘH�̎��͂ɕ��������Ă�̂ŁA��ɒʘH�����ĂĂ���K�v������</summary>
    internal void BuildDungeonRoom(IReadOnlyDictionary<Vector3Int, DungeonPassMassData> passMassDic)
    {
        // ���������Ă���Ƃ��ĒʘH�̑��̍��W�������^�Ŏ󂯎��
        IReadOnlyCollection<Vector3Int> passMassPositions = passMassDic.Keys as IReadOnlyCollection<Vector3Int>;
        IReadOnlyDictionary<Vector3Int, Direction> estimatePosDic = GetAvailablePosDic(passMassPositions);
        // �ʘH��ɕ����𗧂Ă��Ȃ��悤�ɒʘH�̍��W�̃R���N�V���������Ƃɐ�������
        List<Vector3Int> alreadyBuildPosList = new (passMassPositions);

        // �C���X�y�N�^�[�Ŋ��蓖�Ă����ɁA�����\�ȃ����_���Ȉʒu�ɕ����𐶐����Ă���
        int roomIndex = 0;
        foreach (KeyValuePair<Vector3Int, Direction> pair in estimatePosDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            Vector3Int pos = pair.Key;
            Direction dir = pair.Value;

            DungeonRoomData roomData = _roomDatas[roomIndex];
            IReadOnlyCollection<Vector3Int> roomRanges = GetRoomRangeSet(pos, dir, roomData);
            
            if (IsAlreadyBuilded(roomRanges, alreadyBuildPosList)) continue;

            if (roomData.IsAvailable())
            {
                GameObject prefab = roomData.GetRandomVariationPrefab();
                Quaternion rot = _helper.ConvertToInverseRot(dir);
                Instantiate(prefab, pos, rot, _prefabParent);

                // �������m���d�Ȃ�Ȃ��悤�ɐ������������̍��W��ǉ�����
                foreach (Vector3Int v in roomRanges)
                {
                    alreadyBuildPosList.Add(v);
                }

                _roomEntranceDic.Add(pos, dir);
            }
            else if (++roomIndex >= _roomDatas.Length)
            {
                break;
            }
        }
    }

    /// <summary>�ʘH�̑��̍��W�𒲂ׂāA���������Ă�����W�̎����^�Ƃ��ĕԂ�</summary>
    IReadOnlyDictionary<Vector3Int, Direction> GetAvailablePosDic(IReadOnlyCollection<Vector3Int> passMassPositions)
    {
        // �����e�ʂ͒ʘH�̗��e����z�肵�ĒʘH�̐�*2��p�ӂ��Ă���
        Dictionary<Vector3Int, Direction> estimatePosDic = new (passMassPositions.Count * 2);

        foreach (Vector3Int pos in passMassPositions)
        {
            (int binary, _) = _helper.GetNeighbourBinary(pos, passMassPositions);

            // �e�����ɒʘH��������΂��̕����𕔉��𐶐��\�ȏꏊ�Ƃ��Ď����ɒǉ�����
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryForward)) Add(Direction.Forward);
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryBack))    Add(Direction.Back);
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryLeft))    Add(Direction.Left);
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryRight))   Add(Direction.Right);

            void Add(Direction placeDir)
            {
                Vector3Int placePos = pos + _helper.ConvertToPos(placeDir);
                // �d���`�F�b�N
                if (estimatePosDic.ContainsKey(placePos)) return;
                estimatePosDic.Add(placePos, placeDir);
            }
        }

        return estimatePosDic;
    }

    /// <summary>�������镔���͈̔͂��擾����</summary>
    IReadOnlyCollection<Vector3Int> GetRoomRangeSet(Vector3Int pos, Direction dir, DungeonRoomData roomData)
    {
        int depth = roomData.Depth;
        int width = roomData.Width;

        // �J��Ԃ��Ăяo�����\�b�h�Ȃ̂ŉ����new�����ɋ�ɂ��Ďg���܂킷
        _roomRangeList.Clear();

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

        return _roomRangeList;

        // �w�肳�ꂽ���W�Ƃ��̏㉺�������͍��E�̍��W��ǉ����Ă���
        void Add(Vector3Int center, Vector3Int dir)
        {
            // ���_��ǉ�
            _roomRangeList.Add(center);
            // ���E�̕�����ǉ�
            for (int i = 1; i <= width / 2; i++)
            {
                _roomRangeList.Add(center + dir * i * _helper.PrefabScale);
                _roomRangeList.Add(center - dir * i * _helper.PrefabScale);
            }
        }
    }

    bool IsAlreadyBuilded(IReadOnlyCollection<Vector3Int> roomRangeSet, 
                          IReadOnlyCollection<Vector3Int> alreadyBuildPosSet)
    {
        foreach (Vector3Int pos in roomRangeSet)
        {
            if (alreadyBuildPosSet.Contains(pos)) 
                return true;
        }

        return false;
    }
}
