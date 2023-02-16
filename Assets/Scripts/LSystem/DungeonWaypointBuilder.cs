using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンのWaypointを生成するコンポーネント
/// 生成したWaypointはヒエラルキー上のオブジェクトの親子構造を通して参照される
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("デバッグ用:Waypointの可視化に使用するプレハブ")]
    [SerializeField] GameObject _passWaypointVisualizer;
    [SerializeField] GameObject _roomWaypointVisualizer;
    [Header("各Waypointとして生成するプレハブ")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomWaypointPrefab;
    [SerializeField] GameObject _exitWaypointPrefab;
    [Header("生成したWaypointの親")]
    [SerializeField] Transform _waypointParent;

    internal void BuildPassWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _passWaypointPrefab);
        list.ForEach(go => go.transform.parent = _waypointParent);
    }

    internal void BuildRoomWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _roomWaypointPrefab);
        list.ForEach(go => go.transform.parent = _waypointParent);
    }

    internal void BuildExitWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _exitWaypointPrefab);
        list.ForEach(go => go.transform.parent = _waypointParent);
    }

    internal void VisualizePassWaypoint(IEnumerable<Vector3Int> positions)
    {
        BuildWaypoint(positions, _passWaypointVisualizer);
    }

    internal void VisualizeRoomWaypoint(IEnumerable<Vector3Int> positions)
    {
        BuildWaypoint(positions, _roomWaypointVisualizer);
    }

    List<GameObject> BuildWaypoint(IEnumerable<Vector3Int> positions, GameObject prefab)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (Vector3Int pos in positions)
        {
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            list.Add(go);
        }

        return list;
    }
}
