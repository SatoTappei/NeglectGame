using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���W������Waypoint�����o������R���|�[�l���g
/// </summary>
public class DungeonWaypointVisualizer : MonoBehaviour
{
    [Header("�f�o�b�O�p:Waypoint�̉����Ɏg�p����v���n�u")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomEntranceWaypointPrefab;

    internal void VisualizePassWaypoint(IEnumerable<Vector3Int> pos)
    {
        VisualizeWaypoint(pos, _passWaypointPrefab);
    }

    internal void VisualizeRoomWaypoint(IEnumerable<Vector3Int> pos)
    {
        VisualizeWaypoint(pos, _roomEntranceWaypointPrefab);
    }

    void VisualizeWaypoint(IEnumerable<Vector3Int> pos, GameObject prefab)
    {
        foreach (Vector3Int p in pos)
        {
            Instantiate(prefab, p, Quaternion.identity);
        }
    }
}
