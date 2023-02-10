using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動時に辿るノードをStackで取得する処理を実装させるインターフェース
/// </summary>
public interface IPathGetable
{
    public Stack<Vector3> GetPathStack(Vector3 _startPos, Vector3 _targetPos);
}
