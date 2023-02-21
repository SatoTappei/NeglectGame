using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�L�����N�^�[���o�H�T���Ɏg�p����Waypoint���Ǘ�����N���X
/// </summary>
public class ActorPathfindingWaypoint
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;

    public ActorPathfindingWaypoint(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic)
    {
        _waypointDic = waypointDic;
    }

    // TODO:�����E�F�C�|�C���g��A���Ŋl�����Ă��܂��̂������
    public Vector3 Get(WaypointType type)
    {
        List<Vector3> list = _waypointDic[type]; 
        Vector3 waypoint = list[Random.Range(0, list.Count)];

        return waypoint;
    }
}
