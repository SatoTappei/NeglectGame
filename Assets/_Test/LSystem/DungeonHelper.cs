using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ダンジョン生成関係のクラスで使うヘルパークラス
/// </summary>
internal class DungeonHelper
{
    internal enum Direction
    {
        Forward,
        Back,
        Right,
        Left,
    }

    // ダンジョン生成に使うプレハブの大きさ(TransformのScaleとは別)
    internal readonly int PrefabScale = 3;
    // 前後左右の4方向を表すバイナリ
    internal readonly int BForward = 0b1000;
    internal readonly int BBack    = 0b0100;
    internal readonly int BLeft    = 0b0010;
    internal readonly int BRight   = 0b0001;

    /// <summary>隣にオブジェクトが存在する方向をまとめて返す</summary>
    internal HashSet<Direction> GetNeighbour(Vector3Int pos, ICollection<Vector3Int> coll)
    {
        HashSet<Direction> dirSet = new HashSet<Direction>(4);
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale)) dirSet.Add(Direction.Forward);
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))    dirSet.Add(Direction.Back);
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))   dirSet.Add(Direction.Right);
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))    dirSet.Add(Direction.Left);

        return dirSet;
    }

    /// <summary>
    /// 隣にオブジェクトがある方向と接続数をバイナリで返す
    /// 指定された方向が含まれているかどうかの判定はこちらを使う
    /// </summary>
    /// <returns>0b前後左右, 接続数</returns>
    internal (int dirs, int count) GetNeighbourBinary(Vector3Int pos, IReadOnlyCollection<Vector3Int> coll)
    {
        int dirs = 0b0000;
        int count = 0;
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale))
        {
            dirs += BForward;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))
        {
            dirs += BBack;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))
        {
            dirs += BLeft;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))
        {
            dirs += BRight;
            count++;
        }

        return (dirs, count);
    }

    internal Vector3Int ConvertToPos(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Vector3Int.forward * PrefabScale;
            case Direction.Back:
                return Vector3Int.back * PrefabScale;
            case Direction.Left:
                return Vector3Int.left * PrefabScale;
            case Direction.Right:
                return Vector3Int.right * PrefabScale;
            default:
                Debug.LogError("列挙型Directionで定義されていない値です。: " + dir);
                return Vector3Int.zero;
        }
    }

    /// <summary>方向と逆の回転を取得する</summary>
    internal Quaternion ConvertToInverseRot(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Back:
                return Quaternion.Euler(0, 180, 0);
            case Direction.Left:
                return Quaternion.Euler(0, -90, 0);
            case Direction.Right:
                return Quaternion.Euler(0, 90, 0);
            default:
                Debug.LogError("列挙型Directionで定義されていない値です。: " + dir);
                return Quaternion.identity;
        }
    }
}
