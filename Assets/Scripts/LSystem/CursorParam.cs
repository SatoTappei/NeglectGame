using UnityEngine;

/// <summary>
/// LSystemを用いて生成した文字列からダンジョンの通路を生成する際に使う
/// 操作の基準となるパラメータ
/// </summary>
internal class CursorParam
{
    Vector3Int _pos;
    Vector3Int _dirVec;
    int _dist;

    internal CursorParam(Vector3Int pos, Vector3Int dirVec, int dist)
    {
        _pos = pos;
        _dirVec = dirVec;
        _dist = dist;
    }

    internal Vector3Int Pos => _pos;
    internal Vector3Int DirVec => _dirVec;
    internal int Dist => _dist;
}