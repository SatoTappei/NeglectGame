using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;
using ComponentShape = DungeonPassMassData.ComponentShape;

/// <summary>
/// ダンジョンの通路生成時に行うDirectionの値に応じた処理だけを抜き出したクラス
/// </summary>
internal class DungeonPassDirectionCalculator
{
    DungeonHelper _helper;

    internal DungeonPassDirectionCalculator()
    {
        _helper = new();
    }

    internal float GetPassStraightRotY(Vector3Int dirVec)
    {
        Direction dir = _helper.ConvertToDir(dirVec);
        return GetPassStraightRotY(dir);
    }

    internal float GetPassStraightRotY(Direction roomDir)
    {
        if(roomDir == Direction.Left ||
           roomDir == Direction.Right)
        {
            return 90;
        }

        return 0;
        
    }

    internal float GetCornerRotY(Direction roomDir, Direction frontMassDir)
    {
        if((roomDir == Direction.Forward && frontMassDir == Direction.Right) ||
           (roomDir == Direction.Left && frontMassDir == Direction.Back))
        {
            return 90;
        }
        else if((roomDir == Direction.Back && frontMassDir == Direction.Left) ||
                (roomDir == Direction.Right && frontMassDir == Direction.Forward))
        {
            return -90;
        }
        else if((roomDir == Direction.Forward && frontMassDir == Direction.Left) ||
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
}
