using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T����p���Ĉړ��������Ǘ�����R���|�[�l���g
/// </summary>
internal class PathfindingTarget : MonoBehaviour
{
    [SerializeField] Transform[] _targetArr;
    [SerializeField] Transform _exit;

    // TODO:�O��Ɠ����n�_���S�[���Ƃ��đI��ł��܂��ƈړ����Ȃ��Ȃ�s�������̂łǂ��ɂ�����
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
        Debug.Log("����̈ړ����:" + r);

        return _targetArr[r].position;
    }

    internal Vector3 GetExitPos() => _exit.position;
}
