using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームに登場するキャラクターを制御するコンポーネント
/// </summary>
public class Actor : MonoBehaviour
{
    // 意欲が無くなった場合はダンジョンから脱出する
    // 目的を達成した場合もダンジョンから脱出する
    // 影響マップに従って行動する

    [SerializeField] Transform _target;
    float _speed = 0.1f;
    Vector3[] _path;
    int _targetIndex;

    Coroutine _coroutine;

    void Start()
    {
        PathfindingManager.RequestPath(transform.position, _target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }
                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed);
            yield return null;
        }
    }
}
