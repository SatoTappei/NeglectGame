using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 辿るノードが格納されたインターフェース
/// </summary>
public interface IPathGetable
{
    public Stack<Vector3> GetPathStack(Vector3 _startPos, Vector3 _targetPos);
}
