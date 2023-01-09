using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonBuildingHelper.Direction;

/// <summary>
/// 生成したダンジョンの通路を弄るクラス
/// </summary>
public class DungeonPassHelper
{
    // TODO:このクラス自体が本当に必要かどうか検討する
    //      DungeonPassBuilderクラスに統合できないか

    // この定数自体がダブっているので共通で使える便利クラスにする必要がある
    // もしくは引数でスケールをとるか
    readonly int PrefabScale = 3;

    /// <summary>隣にオブジェクトが存在する方向をまとめて返す</summary>
    public HashSet<Direction> GetNeighbour(Vector3Int pos, ICollection<Vector3Int> coll)
    {
        //Debug.Log("-----");
        //Debug.Log(pos + Vector3Int.forward);
        //Debug.Log(pos + Vector3Int.back);
        //Debug.Log(pos + Vector3Int.right);
        //Debug.Log(pos + Vector3Int.left);
        //foreach (var v in coll)
        //{
        //    Debug.Log(v);
        //}
        //Debug.Log("-----");

        HashSet<Direction> dirSet = new HashSet<Direction>(4);
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale)) dirSet.Add(Direction.Forward);
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))    dirSet.Add(Direction.Back);
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))   dirSet.Add(Direction.Right);
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))    dirSet.Add(Direction.Left);
        //if (coll.Contains(pos + Vector3Int.forward * 3))
        //{
        //    Debug.Log("ある");
        //    dirSet.Add(Direction.Forward);
        //}
        //if (coll.Contains(pos + Vector3Int.back * 3))
        //{
        //    Debug.Log("ある");
        //    dirSet.Add(Direction.Back);
        //}
        //if (coll.Contains(pos + Vector3Int.right * 3))
        //{
        //    Debug.Log("ある");
        //    dirSet.Add(Direction.Right);
        //}
        //if (coll.Contains(pos + Vector3Int.left * 3))
        //{
        //    Debug.Log("ある");
        //    dirSet.Add(Direction.Left);
        //}

        return dirSet;
    }
}
