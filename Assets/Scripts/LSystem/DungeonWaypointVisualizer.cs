using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンのWaypointを視覚化するコンポーネント
/// </summary>
public class DungeonWaypointVisualizer : MonoBehaviour
{
    [Header("デバッグ用:Waypointの可視化に使用するプレハブ")]
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
