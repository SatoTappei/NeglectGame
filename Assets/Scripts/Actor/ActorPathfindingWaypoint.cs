using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�L�����N�^�[���o�H�T���Ɏg�p����Waypoint���Ǘ�����R���|�[�l���g
/// </summary>
public class ActorPathfindingWaypoint : MonoBehaviour
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;

    public void Init(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic)
    {
        if (_waypointDic != null)
        {
            Debug.LogWarning("���ɏ������ς݂ł�");
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
