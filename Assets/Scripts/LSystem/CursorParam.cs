using UnityEngine;

/// <summary>
/// LSystemを用いて生成した文字列からダンジョンの通路を生成する際に使う
/// 操作の基準となるパラメータ
/// </summary>
internal class CursorParam
{
    internal CursorParam(Vector3Int pos, Vector3Int dirVec, int dist)
    {
        Pos = pos;
        DirVec = dirVec;
        Dist = dist;
    }

    internal Vector3Int Pos { get; }
    internal Vector3Int DirVec { get; }
    internal int Dist { get; }
}