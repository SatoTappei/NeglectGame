using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*�Ŏg���O���b�h��̃}�b�v�ɕ~���l�߂���e�m�[�h�̃N���X
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
