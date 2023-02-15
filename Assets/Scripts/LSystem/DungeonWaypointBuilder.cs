using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンのWaypointを視覚化するコンポーネント
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("デバッグ用:Waypointの可視化に使用するプレハブ")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomEntranceWaypointPrefab;

    public void VisualizeWaypoint(IReadOnlyCollection<Vector3Int> pos)
    {
        foreach (Vector3Int p in pos)
        {
            Instantiate(_passWaypointPrefab, p, Quaternion.identity);
        }
    }
}
