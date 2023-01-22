using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���ɓo�ꂷ��L�����N�^�[�𐧌䂷��R���|�[�l���g
/// </summary>
public class Actor : MonoBehaviour
{
    // �ӗ~�������Ȃ����ꍇ�̓_���W��������E�o����
    // �ړI��B�������ꍇ���_���W��������E�o����
    // �e���}�b�v�ɏ]���čs������

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
