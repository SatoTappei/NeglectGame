using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ダンジョン生成関係のクラスで使うヘルパークラス
/// </summary>
internal class DungeonHelper
{
    /// <summary>ダンジョン生成に使うプレハブの大きさ(TransformのScaleとは別)</summary>
    internal static readonly int PrefabScale = 3;
    /// <summary>前後左右の4方向を表すバイナリ</summary>
    internal static readonly int BinaryForward = 0b1000;
    internal static readonly int BinaryBack    = 0b0100;
    internal static readonly int BinaryLeft    = 0b0010;
    internal static readonly int BinaryRight   = 0b0001;

    internal int GetConnectedFromShape(ComponentShape shape)
    {
        switch (shape)
        {
            case ComponentShape.PassEnd:   return 1;
            case ComponentShape.Pass:      return 2;
            case ComponentShape.Corner:    return 2;
            case ComponentShape.TJunction: return 3;
            case ComponentShape.Cross:     return 4;
            default:
                Debug.LogError("値が不正です: " + shape);
                return -1;
        }
    }

    /// <summary>
    /// どの方向にいくつ接続されているかの判定はわざわざコレクションを生成しなくても良い
    /// </summary>
    /// <returns>0b前後左右, 接続数</returns>
    internal (int, int) GetNeighbourBinary(Vector3Int centerPos, IReadOnlyCollection<Vector3Int> positions)
    {
        int binary = 0b0000;
        int count = 0;

        if (positions.Contains(centerPos + Vector3Int.forward * PrefabScale))
        {
            binary += BinaryForward;
            count++;
        }
        if (positions.Contains(centerPos + Vector3Int.back * PrefabScale))
        {
            binary += BinaryBack;
            count++;
        }
        if (positions.Contains(centerPos + Vector3Int.left * PrefabScale))
        {
            binary += BinaryLeft;
            count++;
        }
        if (positions.Contains(centerPos + Vector3Int.right * PrefabScale))
        {
            binary += BinaryRight;
            count++;
        }

        return (binary, count);
    }

    internal bool IsConnectedUsingBinary(int neighbour, int center) => (neighbour & center) == center;

    internal Vector3Int GetDirectionPos(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Vector3Int.forward * PrefabScale;
            case Direction.Back:    return Vector3Int.back * PrefabScale;
            case Direction.Left:    return Vector3Int.left * PrefabScale;
            case Direction.Right:   return Vector3Int.right * PrefabScale;
            default:
                Debug.LogError("列挙型Directionで定義されていない値です: " + dir);
                return Vector3Int.zero;
        }
    }

    internal Quaternion GetDirectionAngle(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Quaternion.Euler(0, 0, 0);
            case Direction.Back:    return Quaternion.Euler(0, 180, 0);
            case Direction.Left:    return Quaternion.Euler(0, -90, 0);
            case Direction.Right:   return Quaternion.Euler(0, 90, 0);
            default:
                Debug.LogError("列挙型Directionで定義されていない値です: " + dir);
                return Quaternion.identity;
        }
    }

    internal Direction GetDirection(float rotY)
    {
        if      (rotY == 0)   return GetDirection(Vector3Int.forward);
        else if (rotY == 180) return GetDirection(Vector3Int.back);
        else if (rotY == -90) return GetDirection(Vector3Int.left);
        else if (rotY == 90)  return GetDirection(Vector3Int.right);
        else
        {
            Debug.LogError("floatの値が不正です: " + rotY);
            return Direction.Forward;
        }
    }

    internal Direction GetDirection(Vector3Int dirVec)
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

    /// <summary>渡された方向ベクトルから90度回転させた方向ベクトルを返す</summary>
    /// <param name="isPositive">trueだと前右後左の時計回り、falseだと反時計回り</param>
    internal Vector3Int GetDirectionVectorRotate90(Vector3Int dirVec, bool isPositive)
    {
        if      (dirVec == Vector3Int.forward) return isPositive ? Vector3Int.right : Vector3Int.left;
        else if (dirVec == Vector3Int.right)   return isPositive ? Vector3Int.back : Vector3Int.forward;
        else if (dirVec == Vector3Int.back)    return isPositive ? Vector3Int.left : Vector3Int.right;
        else if (dirVec == Vector3Int.left)    return isPositive ? Vector3Int.forward : Vector3Int.back;
        else
        {
            Debug.LogError("上下左右以外の角度です: " + dirVec);
            return Vector3Int.zero;
        }
    }

    internal bool IsPassStraight(int dirs)
    {
        if (IsConnectedUsingBinary(dirs, BinaryForward) && IsConnectedUsingBinary(dirs, BinaryBack) ||
            IsConnectedUsingBinary(dirs, BinaryLeft) && IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal float GetPassStraightRotY(Direction roomDir)
    {
        if (roomDir == Direction.Left || roomDir == Direction.Right)
        {
            return 90;
        }
        else
        {
            return 0;
        }
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
        else
        {
            return 0;
        }
    }

    internal float GetCornerRotY(int dirs)
    {
        if (IsConnectedUsingBinary(dirs, BinaryForward) && IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return 180;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryForward) && IsConnectedUsingBinary(dirs, BinaryLeft))
        {
            return 90;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryBack) && IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return -90;
        }
        else
        {
            return 0;
        }
    }

    internal float GetTJunctionRotY(Direction roomDir, Direction frontMassDir, ComponentShape frontMassShape)
    {
        // 通路に部屋が隣接して生成されるパターン
        if (frontMassShape == ComponentShape.Pass)
        {
            if      (roomDir == Direction.Forward) return 180;
            else if (roomDir == Direction.Left)    return 90;
            else if (roomDir == Direction.Right)   return -90;
        }
        // 通路の端で部屋2つが挟み込むパターン
        else if (frontMassShape == ComponentShape.PassEnd)
        {
            if      (frontMassDir == Direction.Back)  return 180;
            else if (frontMassDir == Direction.Left)  return -90;
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

    internal float GetTJunctionRotY(int dirs)
    {
        if (IsConnectedUsingBinary(dirs, BinaryForward) && 
            IsConnectedUsingBinary(dirs, BinaryBack) && 
            IsConnectedUsingBinary(dirs, BinaryLeft))
        {
            return 90;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryForward) && 
                 IsConnectedUsingBinary(dirs, BinaryBack) && 
                 IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return -90;
        }
        else if (IsConnectedUsingBinary(dirs, BinaryForward) && 
                 IsConnectedUsingBinary(dirs, BinaryLeft) && 
                 IsConnectedUsingBinary(dirs, BinaryRight))
        {
            return 180;
        }

        return 0;
    }

    internal float GetPassEndRotY(int dirs)
    {
        if      (IsConnectedUsingBinary(dirs, BinaryForward)) return 180;
        else if (IsConnectedUsingBinary(dirs, BinaryLeft)) return 90;
        else if (IsConnectedUsingBinary(dirs, BinaryRight)) return -90;

        return 0;
    }
}
