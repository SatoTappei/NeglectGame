using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*で使うグリッド状のマップに敷き詰められる各ノードのクラス
/// </summary>
internal class Node
{
    internal Node(Vector3 pos, bool isMovable, int gridX, int gridZ)
    {
        Pos = pos;
        IsMovable = isMovable;
        GridX = gridX;
        GridZ = gridZ;
    }

    internal Vector3 Pos { get; }
    internal bool IsMovable { get; }
    internal int GridX { get; }
    internal int GridZ { get; }
    internal int ActualCost { get; set; }
    internal int EstimateCost { get; set; }
    internal Node ParentNode { get; set; }
    internal int TotalCost => ActualCost + EstimateCost;
}
