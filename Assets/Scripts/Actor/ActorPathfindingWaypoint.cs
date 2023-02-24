using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各キャラクターが経路探索に使用するWaypointを管理するクラス
/// </summary>
public class ActorPathfindingWaypoint
{
    static readonly int TryGetRandomWaypointIteration = 10;

    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;
    Vector3 _prevPassWaypoint;
    
    public Vector3 ExitPos { get; }

    public ActorPathfindingWaypoint(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic, Vector3 exitPos)
    {
        _waypointDic = waypointDic;
        ExitPos = exitPos;

        // 最初のこのフィールドへの参照で値が同じにならないよう、ありえない値で初期化しておく
        _prevPassWaypoint = Vector3.one * -999;
    }

    public Vector3 Get(WaypointType type)
    {
        List<Vector3> list = _waypointDic[type];

        Vector3 waypoint = Vector3.zero;
        for (int i = 0; i < TryGetRandomWaypointIteration; i++)
        {
            waypoint = list[Random.Range(0, list.Count)];
            if (waypoint != _prevPassWaypoint)
            {
                _prevPassWaypoint = waypoint;
                return waypoint;
            }
        }

        return waypoint;
    }
}
