using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ダンジョン生成関係のクラスで使うヘルパークラス
/// </summary>
internal class DungeonHelper
{
    // ダンジョン生成に使うプレハブの大きさ(TransformのScaleとは別)
    internal readonly int PrefabScale = 3;
    // 前後左右の4方向を表すバイナリ
    internal const int BinaryForward = 0b1000;
    internal const int BinaryBack    = 0b0100;
    internal const int BinaryLeft    = 0b0010;
    internal const int BinaryRight   = 0b0001;

    internal int GetConnectedFromShape(ComponentShape shape)
    {
        switch (shape)
        {
            case ComponentShape.PassEnd:   return 1;
            case ComponentShape.Pass:      return 2;
            case ComponentShape.Corner:    return 2;
            case ComponentShape.TJunction: return 3;
            case ComponentShape.Cross:     return 4;
        }

        return -1;
    }

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
    internal (int, int) GetNeighbourBinary(Vector3Int pos, IReadOnlyCollection<Vector3Int> coll)
    {
        int binary = 0b0000;
        int count = 0;
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale))
        {
            binary += BinaryForward;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))
        {
            binary += BinaryBack;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))
        {
            binary += BinaryLeft;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))
        {
            binary += BinaryRight;
            count++;
        }

        return (binary, count);
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

    internal Direction ConvertToDirection(Vector3Int dirVec)
    {
        if      (dirVec == Vector3Int.forward) return Direction.Forward;
        else if (dirVec == Vector3Int.back)    return Direction.Back;
        else if (dirVec == Vector3Int.left)    return Direction.Left;
        else if (dirVec == Vector3Int.right)   return Direction.Right;
        else
        {
            Debug.LogError("方向ベクトルの値が不正です: " + dirVec);
            return Direction.Forward;
        }
    }

    internal Direction ConvertToDirection(float rotY)
    {
        if      (rotY == 0)   return Direction.Forward;
        else if (rotY == 180) return Direction.Back;
        else if (rotY == -90) return Direction.Left;
        else if (rotY == 90)  return Direction.Right;
        else
        {
            Debug.LogError("floatの値が不正です: " + rotY);
            return Direction.Forward;
        }
    }

    /// <summary>渡された方向ベクトルから90度回転させた方向ベクトルを返す</summary>
    /// <param name="isPositive">trueだと前右後左の時計回り、falseだと反時計回り</param>
    internal Vector3Int GetRotate90(Vector3Int dirVec, bool isPositive)
    {
        if      (dirVec == Vector3Int.forward) return isPositive ? Vector3Int.right : Vector3Int.left;
        else if (dirVec == Vector3Int.right)   return isPositive ? Vector3Int.back : Vector3Int.forward;
        else if (dirVec == Vector3Int.back)    return isPositive ? Vector3Int.left : Vector3Int.right;
        else if (dirVec == Vector3Int.left)    return isPositive ? Vector3Int.forward : Vector3Int.back;

        Debug.LogError("上下左右以外の角度です。: " + dirVec);
        return Vector3Int.zero;
    }

    internal float GetPassStraightRotY(Direction roomDir)
    {
        if (roomDir == Direction.Left ||
           roomDir == Direction.Right)
        {
            return 90;
        }

        return 0;
    }

    internal float GetCornerRotY(Direction roomDir, Direction frontMassDir)
    {
        if ((roomDir == Direction.Forward && frontMassDir == Direction.Right) ||
           (roomDir == Direction.Left && frontMassDir == Direction.Back))
        {
            return 90;
        }
        else if ((roomDir == Direction.Back && frontMassDir == Direction.Left) ||
                (roomDir == Direction.Right && frontMassDir == Direction.Forward))
        {
            return -90;
        }
        else if ((roomDir == Direction.Forward && frontMassDir == Direction.Left) ||
                (roomDir == Direction.Right && frontMassDir == Direction.Back))
        {
            return 180;
        }

        return 0;
    }

    internal float GetTJunctionRotY(Direction roomDir, Direction frontMassDir, ComponentShape frontMassShape)
    {
        // 通路に部屋が隣接して生成されるパターン
        if (frontMassShape == ComponentShape.Pass)
        {
            if (roomDir == Direction.Forward) return 180;
            else if (roomDir == Direction.Left) return 90;
            else if (roomDir == Direction.Right) return -90;
        }
        // 通路の端で部屋2つが挟み込むパターン
        else if (frontMassShape == ComponentShape.PassEnd)
        {
            if (frontMassDir == Direction.Back) return 180;
            else if (frontMassDir == Direction.Left) return -90;
            else if (frontMassDir == Direction.Right) return 90;
        }
        // 通路の角に部屋が生成されるパターン
        else if (frontMassShape == ComponentShape.Corner)
        {
            if ((roomDir == Direction.Forward && frontMassDir == Direction.Forward) ||
                (roomDir == Direction.Back && frontMassDir == Direction.Right))
            {
                return 90;
            }
            else if ((roomDir == Direction.Forward && frontMassDir == Direction.Left) ||
                     (roomDir == Direction.Back && frontMassDir == Direction.Back))
            {
                return -90;
            }
            else if (roomDir == Direction.Left && frontMassDir == Direction.Back)
            {
                return -180;
            }
            else if (roomDir == Direction.Right && frontMassDir == Direction.Right)
            {
                return 180;
            }
        }

        return 0;
    }

    // ばいなり計算機クラス
    const int Forward = DungeonHelper.BinaryForward;
    const int Back = DungeonHelper.BinaryBack;
    const int Left = DungeonHelper.BinaryLeft;
    const int Right = DungeonHelper.BinaryRight;

    bool IsConnect(int dirs, int BinaryDir) => /*_helper.*/IsConnectFromBinary(dirs, BinaryDir);

    internal float GetPassEndRotY(int dirs)
    {
        if (IsConnect(dirs, Forward)) return 180;
        else if (IsConnect(dirs, Left)) return 90;
        else if (IsConnect(dirs, Right)) return -90;

        return 0;
    }

    internal bool IsPassStraight(int dirs)
    {
        if (IsConnect(dirs, Forward) && IsConnect(dirs, Back) ||
            IsConnect(dirs, Left) && IsConnect(dirs, Right))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal float GetCornerRotY(int dirs)
    {
        if (IsConnect(dirs, Forward) && IsConnect(dirs, Right)) return 180;
        else if (IsConnect(dirs, Forward) && IsConnect(dirs, Left)) return 90;
        else if (IsConnect(dirs, Back) && IsConnect(dirs, Right)) return -90;

        return 0;
    }

    internal float GetTJunctionRotY(int dirs)
    {
        if (IsConnect(dirs, Forward) && IsConnect(dirs, Back) && IsConnect(dirs, Left)) return 90;
        else if (IsConnect(dirs, Forward) && IsConnect(dirs, Back) && IsConnect(dirs, Right)) return -90;
        else if (IsConnect(dirs, Forward) && IsConnect(dirs, Left) && IsConnect(dirs, Right)) return 180;

        return 0;
    }
}
