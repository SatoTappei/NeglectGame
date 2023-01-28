using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索を用いて移動する先を管理するコンポーネント
/// </summary>
internal class PathfindingTarget : MonoBehaviour
{
    [SerializeField] Transform[] _targetArr;

    // TODO:前回と同じ地点をゴールとして選んでしまうと移動しなくなる不具合があるのでどうにかする
    int _prev = -1;

    internal Vector3 GetPathfindingTarget()
    {
        int r;
        while (true)
        {
            r = Random.Range(0, _targetArr.Length);
            if (r != _prev)
            {
                _prev = r;
                break;
            }
        }
        Debug.Log("今回の移動先は:" + r);

        return _targetArr[r].position;
    }
}
