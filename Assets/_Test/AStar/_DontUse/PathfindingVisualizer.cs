using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索の情報をギズモに表示するコンポーネント
/// 未使用
/// </summary>
public class PathfindingVisualizer : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [Header("グリッド位置を表示するキャラクター")]
    [SerializeField] Transform _character;

    void OnDrawGizmos()
    {
        //if (_pathfindingGrid == null || _character == null || _pathfindingGrid.Grid == null)
        //{
        //    Debug.LogWarning("グリッド情報を表示するための参照が不足しています。");
        //    return;
        //}

        //Node characterNode = _pathfindingGrid.GetNode(_character.position);
        //foreach (Node node in _pathfindingGrid.Grid)
        //{
        //    if (characterNode == node)
        //        Gizmos.color = Color.green;
        //    else if (node.IsMovable)
        //        Gizmos.color = Color.white;
        //    else
        //        Gizmos.color = Color.red;

        //    Gizmos.DrawCube(node.Pos, Vector3.one * _pathfindingGrid.NodeDiameter() * .9f);
        //}

        //if (_path != null)
        //{
        //    for (int i = _targetIndex; i < _path.Length; i++)
        //    {
        //        Gizmos.color = Color.black;
        //        Gizmos.DrawCube(_path[i], Vector3.one);

        //        if (i == _targetIndex)
        //        {
        //            Gizmos.DrawLine(transform.position, _path[i]);
        //        }
        //        else
        //        {
        //            Gizmos.DrawLine(_path[i - 1], _path[i]);
        //        }
        //    }
        //}
    }
}
