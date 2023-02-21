using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索処理を実装させるインターフェース
/// </summary>
public interface IPathfinding
{
    Stack<Vector3> GetPathToTargetPos(Vector3 startPos, Vector3 endPos);
}
