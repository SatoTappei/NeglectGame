using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LSystem‚Ì‹T‚Ìƒpƒ‰ƒ[ƒ^
/// </summary>
public class TurtleParam
{
    Vector3Int _pos;
    Vector3Int _dir;
    int _dist;

    public TurtleParam(Vector3Int pos, Vector3Int dir, int dist)
    {
        _pos = pos;
        _dir = dir;
        _dist = dist;
    }

    public Vector3Int Pos => _pos;
    public Vector3Int Dir => _dir;
    public int Dist => _dist;
}
