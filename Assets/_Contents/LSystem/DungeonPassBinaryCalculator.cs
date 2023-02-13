using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンの通路生成時に行うint型のバイナリに応じた処理だけを抜き出したクラス
/// </summary>
internal class DungeonPassBinaryCalculator
{
    DungeonHelper _helper;
    const int Forward = DungeonHelper.BinaryForward;
    const int Back = DungeonHelper.BinaryBack;
    const int Left = DungeonHelper.BinaryLeft;
    const int Right = DungeonHelper.BinaryRight;

    internal DungeonPassBinaryCalculator()
    {
        _helper = new();
    }

    bool IsConnect(int dirs, int BinaryDir) => _helper.IsConnectFromBinary(dirs, BinaryDir);

    internal float GetPassEndRotY(int dirs)
    {
        if      (IsConnect(dirs, Forward)) return 180;
        else if (IsConnect(dirs, Left))    return 90;
        else if (IsConnect(dirs, Right))   return -90;
        
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
        if      (IsConnect(dirs, Forward) && IsConnect(dirs, Right)) return 180;
        else if (IsConnect(dirs, Forward) && IsConnect(dirs, Left))  return 90;
        else if (IsConnect(dirs, Back) && IsConnect(dirs, Right))    return -90;

        return 0;
    }

    internal float GetTJunctionRotY(int dirs)
    {
        if      (IsConnect(dirs, Forward) && IsConnect(dirs, Back) && IsConnect(dirs, Left))  return 90;
        else if (IsConnect(dirs, Forward) && IsConnect(dirs, Back) && IsConnect(dirs, Right)) return -90;
        else if (IsConnect(dirs, Forward) && IsConnect(dirs, Left) && IsConnect(dirs, Right)) return 180;
        
        return 0;
    }
}
