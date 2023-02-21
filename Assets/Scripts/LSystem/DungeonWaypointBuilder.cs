using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// ダンジョンのWaypointを生成するコンポーネント
/// 生成したWaypointはヒエラルキー上のオブジェクトの親子構造を通して
/// Waypoint側のコンポーネントから参照される
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("各Waypointとして生成するプレハブ")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomWaypointPrefab;
    [SerializeField] GameObject _exitWaypointPrefab;
    [Header("生成したWaypointの親とするオブジェクト")]
    [SerializeField] Transform _waypointParent;
    [Header("生成する出口の数")]
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
            Debug.LogWarning("生成する出口の数が生成できる箇所より多いです");
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
