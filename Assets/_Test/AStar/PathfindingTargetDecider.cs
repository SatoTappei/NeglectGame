using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T���ňړ����������肷��R���|�[�l���g
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
