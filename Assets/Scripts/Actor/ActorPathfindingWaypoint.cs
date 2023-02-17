using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各キャラクターが経路探索に使用するWaypointを管理するコンポーネント
/// </summary>
public class ActorPathfindingWaypoint : MonoBehaviour
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;

    public void Init(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic)
    {
        if (_waypointDic != null)
        {
            Debug.LogWarning("既に初期化済みです");
        }
        else
        {
            _waypointDic = waypointDic;
        }
    }

    public Vector3 GetPassWaypoint()
    {
        List<Vector3> list = _waypointDic[WaypointType.Pass]; 
        Vector3 waypoint = list[Random.Range(0, list.Count)];

        return waypoint;
    }
}
