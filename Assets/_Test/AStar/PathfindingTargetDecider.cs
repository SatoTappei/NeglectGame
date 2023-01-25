using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索で移動する先を決定するコンポーネント
/// </summary>
internal class PathfindingTargetDecider : MonoBehaviour
{
    [SerializeField] Transform[] _targetArr;

    internal Vector3 GetPathfindingTarget()
    {
        int r = Random.Range(0, _targetArr.Length);
        return _targetArr[r].position;
    }
}
