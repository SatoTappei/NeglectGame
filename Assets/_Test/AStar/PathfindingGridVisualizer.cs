using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索のグリッド情報をギズモに表示するコンポーネント
/// </summary>
public class PathfindingGridVisualizer : MonoBehaviour
{
    // TODO:現在Gridクラスにあるものをこっちに移す
    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, _gizmosGridSize);

        //if (_grid != null)
        //{
        //    Node playerNode = GetNode(_player.position);

        //    foreach (Node node in _grid)
        //    {
        //        // 参照型なので配列に新たに追加せずとも同じ参照ならという分岐が出来る
        //        if (playerNode == node)
        //        {
        //            Gizmos.color = Color.cyan;
        //        }
        //        else
        //        {
        //            Gizmos.color = node.IsMovable ? Color.white : Color.red;
        //            //if (path != null)
        //            //    if (path.Contains(node))
        //            //        Gizmos.color = Color.black;
        //        }
        //        Gizmos.DrawCube(node.Pos, Vector3.one * NodeDiameter() * .9f);
        //    }
        //}
    }
}
