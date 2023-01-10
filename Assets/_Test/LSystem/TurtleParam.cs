using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LSystemの亀のパラメータのクラス
/// </summary>
internal class TurtleParam
{
    Vector3Int _pos;
    Vector3Int _dir;
    int _dist;

    internal TurtleParam(Vector3Int pos, Vector3Int dir, int dist)
    {
        _pos = pos;
        _dir = dir;
        _dist = dist;
    }

    internal Vector3Int Pos => _pos;
    internal Vector3Int Dir => _dir;
    internal int Dist => _dist;
}