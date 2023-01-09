using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Direction = DungeonBuildingHelper.Direction;

/// <summary>
/// 生成したダンジョンの通路を弄るクラス
/// </summary>
public class DungeonPassHelper
{
    // TODO:このクラス自体が本当に必要かどうか検討する
    //      DungeonPassBuilderクラスに統合できないか

    // 定数自体がダブっているので共通で使える便利クラスにする必要がある
    // もしくは引数でスケールをとるか
    readonly int PrefabScale = 3;
    readonly int bForward = 0b1000;
    readonly int bBack    = 0b0100;
    readonly int bLeft    = 0b0010;
    readonly int bRight   = 0b0001;

    /// <summary>隣にオブジェクトが存在する方向をまとめて返す</summary>
    public HashSet<Direction> GetNeighbour(Vector3Int pos, ICollection<Vector3Int> coll)
    {
        HashSet<Direction> dirSet = new HashSet<Direction>(4);
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale)) dirSet.Add(Direction.Forward);
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))    dirSet.Add(Direction.Back);
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))   dirSet.Add(Direction.Right);
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))    dirSet.Add(Direction.Left);

        return dirSet;
    }

    /// <summary>
    /// 隣にオブジェクトがある方向と接続数をバイナリ形式で返す
    /// 指定された方向が含まれているかどうかの判定はこちらを使う
    /// </summary>
    /// <returns>0b前後左右, 接続数</returns>
    public (int dirs, int count) GetNeighbourInt(Vector3Int pos, IReadOnlyCollection<Vector3Int> coll)
    {
        int dirs = 0b0000;
        int count = 0;
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale))
        {
            dirs += bForward;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))
        {
            dirs += bBack;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))
        {
            dirs += bLeft;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))
        {
            dirs += bRight;
            count++;
        }

        return (dirs, count);
    }
}
