using System;
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

    [Header("�����̃v���n�u")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("���������v���n�u�̐e")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _roomDic;

    void Awake()
    {
        _helper = new DungeonHelper();
        _roomDic = new Dictionary<Vector3Int, GameObject>(RoomDicCap);
    }

    /// <summary>�����̐������s��</summary>
    internal void GenerateRoom(IReadOnlyCollection<Vector3Int> passAll)
    {
        // �����𐶐��\�ȍ��W�̎������쐬����
        Dictionary<Vector3Int, Direction> placeDic = new Dictionary<Vector3Int, Direction>(PlaceDicCap);
        InsertToDic(placeDic, passAll);

        //List<Vector3Int> blockPosSet = new List<Vector3Int>(BlockPosSetCap);
        int index = 0;
        foreach (KeyValuePair<Vector3Int, Direction> pair in placeDic)
        {
            // ���̍��W�ɐݒu���镔�������肷��
            // ���S�ȃ����_���ł͂Ȃ��A�{�X�����A���󕔉��ȂǕK�v�ȑS�Ă̎�ނ̕�����1���������悤�ɂ���
            // �K�v�ȕ������S���������ꂽ��A�}�X�g����Ȃ������𐶐�����

            DungeonRoomData data = _roomDataArr[index];
            Quaternion rot = GetInverseRot(pair.Value);

            Instantiate(data.GetPrefab(), pair.Key, rot, _parent);

            // ���̕�������̂��Ă�����W�̏ꍇ�͐������Ȃ�
            //if (blockPosSet.Contains(pair.Key)) continue;

            //Quaternion rot = GetInverseRot(pair.Value);

            //// ��������v���t�@�N�^�����O
            //// 2*2�̏ꍇ�͌v4�}�X�A3*3�̏ꍇ�͌v9�}�X���ׂ�
            //for(int i = 0; i < _roomDataArr.Length; i++)
            //{
            //    DungeonRoomData room = _roomDataArr[i];

            //    if (room.MaxQuantity == -1)
            //    {
            //        GameObject go = Instantiate(room.GetPrefab(), pair.Key, rot, _parent);
            //        _roomDic.Add(pair.Key, go);
            //        break;
            //    }

            //    if (room.CheckAvailable())
            //    {
            //        if (room.Size > 1)
            //        {
            //            // �u���b�N����̈�̌v�Z
            //            int length = Mathf.CeilToInt(room.Size / 2);
            //            List<Vector3Int> tempList = new List<Vector3Int>();
            //            if (Fits(length, placeDic, pair, ref tempList))
            //            {
            //                blockPosSet.AddRange(tempList);
            //                var building = Instantiate(room.GetPrefab(), pair.Key, rot);
            //                _roomDic.Add(pair.Key, building);

            //                // �]���̕����������Ƃ��Ēǉ�����
            //                foreach(var pos in tempList)
            //                {
            //                    _roomDic.Add(pos, building);
            //                }
            //            }

            //            GameObject go = Instantiate(room.GetPrefab(), pair.Key, rot, _parent);
            //            _roomDic.Add(pair.Key, go);
            //        }
            //        else
            //        {
            //            GameObject go = Instantiate(room.GetPrefab(), pair.Key, rot, _parent);
            //            _roomDic.Add(pair.Key, go);
            //        }
            //        break;
            //    }
            //}
            // ���t�@�N�^�����O�����܂�
        }
    }

    // ���t�@�N�^�����O�K�v���\�b�h
    bool Fits(int length, Dictionary<Vector3Int, Direction> placeDic,
        KeyValuePair<Vector3Int, Direction> pair, ref List<Vector3Int> tempList)
    {
        Vector3Int dir = Vector3Int.zero;
        // �O���������͌������̏ꍇ�͍��E�Ƀ}�[�W�����K�v
        if (pair.Value == Direction.Forward || pair.Value == Direction.Back)
        {
            dir = Vector3Int.right;
        }
        else
        {
            // ���E�����̏ꍇ�͏㉺�Ƀ}�[�W�����K�v
            dir = new Vector3Int(0, 0, 1);
        }

        // ���̕����̕��̔��a���̃��[�v���K�v
        for(int i = 1; i < length; i++)
        {
            // ���S���獶�E�ɒ��ׂĂ���
            Vector3Int pos1 = pair.Key + dir * i;
            Vector3Int pos2 = pair.Key - dir * i;

            // ���̈ʒu�����ɖ��܂��Ă���ꍇ��false��Ԃ�
            if (!placeDic.ContainsKey(pos1) || !placeDic.ContainsKey(pos2))
            {
                return false;
            }

            // ���܂��Ă��Ȃ��ꍇ�͂����ɕ����𐶐��ł���̂ňꎞ�ۑ����X�g�ɒǉ�����
            tempList.Add(pos1);
            tempList.Add(pos2);
        }
        return true;
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
