using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// �_���W������Waypoint�𐶐�����R���|�[�l���g
/// ��������Waypoint�̓q�G�����L�[��̃I�u�W�F�N�g�̐e�q�\����ʂ���
/// Waypoint���̃R���|�[�l���g����Q�Ƃ����
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("�eWaypoint�Ƃ��Đ�������v���n�u")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomWaypointPrefab;
    [SerializeField] GameObject _exitWaypointPrefab;
    [Header("��������Waypoint�̐e�Ƃ���I�u�W�F�N�g")]
    [SerializeField] Transform _waypointParent;
    [Header("��������o���̐�")]
    [SerializeField] int _exitWaypointQuantity = 1;

    internal void BuildPassWaypoint(IEnumerable<Vector3Int> positions)
    {
        foreach (Vector3Int pos in positions)
        {
            Instantiate(_passWaypointPrefab, pos, Quaternion.identity, _waypointParent);
        }
    }

    internal void BuildRoomWaypoint(IEnumerable<Vector3Int> positions)
    {
        foreach (Vector3Int pos in positions)
        {
            Instantiate(_roomWaypointPrefab, pos, Quaternion.identity, _waypointParent);
        }
    }

    internal void BuildExitWaypointRandom(IEnumerable<Vector3Int> positions)
    {
        if (positions.Count() < _exitWaypointQuantity)
        {
            Debug.LogWarning("��������o���̐��������ł���ӏ���葽���ł�");
        }

        int count = 0;
        foreach (Vector3Int pos in positions.OrderBy(_ => Guid.NewGuid()))
        {
            Instantiate(_exitWaypointPrefab, pos, Quaternion.identity, _waypointParent);

            count++;
            if (count >= _exitWaypointQuantity) break;
        }
    }


}
