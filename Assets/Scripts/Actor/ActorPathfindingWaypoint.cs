using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各キャラクターが経路探索に使用するWaypointを管理するクラス
/// </summary>
public class ActorPathfindingWaypoint
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;

    public ActorPathfindingWaypoint(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic)
    {
        _waypointDic = waypointDic;
    }

    // TODO:同じウェイポイントを連続で獲得してしまうのを避ける
    public Vector3 Get(WaypointType type)
    {
        List<Vector3> list = _waypointDic[type]; 
        Vector3 waypoint = list[Random.Range(0, list.Count)];

        return waypoint;
    }
}
