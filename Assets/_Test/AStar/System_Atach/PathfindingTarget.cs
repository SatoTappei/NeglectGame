using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索を用いて移動する先を管理するコンポーネント
/// </summary>
internal class PathfindingTarget : MonoBehaviour
{
    [SerializeField] Transform[] _targetArr;

    internal Vector3 GetPathfindingTarget()
    {
        int r = Random.Range(0, _targetArr.Length);
        return _targetArr[r].position;
    }
}
