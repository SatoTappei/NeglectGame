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
    internal const int BinaryForward = 0b1000;
    internal const int BinaryBack    = 0b0100;
    internal const int BinaryLeft    = 0b0010;
    internal const int BinaryRight   = 0b0001;

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
            dirs += BinaryForward;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))
        {
            dirs += BinaryBack;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))
        {
            dirs += BinaryLeft;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))
        {
            dirs += BinaryRight;
            count++;
        }

        return (dirs, count);
    }

    internal bool IsConnectFromBinary(int dirs, int BinaryDir) => (dirs & BinaryDir) == BinaryDir;

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

    internal Direction ConvertToDir(Vector3Int dirVec)
    {
        if (dirVec == Vector3Int.forward) return Direction.Forward;
        else if (dirVec == Vector3Int.back) return Direction.Back;
        else if (dirVec == Vector3Int.left) return Direction.Left;
        else if (dirVec == Vector3Int.right) return Direction.Right;
        else
        {
            Debug.LogError("方向ベクトルの値が不正です: " + dirVec);
            return Direction.Forward;
        }
    }

    internal Direction ConvertToDir(float rotY)
    {
        if (rotY == 0) return Direction.Forward;
        else if (rotY == 180) return Direction.Back;
        else if (rotY == -90) return Direction.Left;
        else if (rotY == 90) return Direction.Right;
        else
        {
            Debug.LogError("floatの値が不正です: " + rotY);
            return Direction.Forward;
        }
    }
}
