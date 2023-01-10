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

    [Header("�����̃v���n�u")]
    [SerializeField] GameObject _roomPrefab;
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

        foreach (var pair in placeDic)
        {
            Quaternion rot = GetInverseRot(pair.Value);
            Instantiate(_roomPrefab, pair.Key, rot, _parent);
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
