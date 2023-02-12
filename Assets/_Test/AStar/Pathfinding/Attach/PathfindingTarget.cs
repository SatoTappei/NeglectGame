using UnityEngine;

/// <summary>
/// �o�H�T����p���Ĉړ��������Ǘ�����R���|�[�l���g
/// </summary>
internal class PathfindingTarget : MonoBehaviour, ITargetSelectable
{
    [SerializeField] Transform[] _targetArr;
    [SerializeField] Transform _exit;

    // TODO:�O��Ɠ����n�_���S�[���Ƃ��đI��ł��܂��ƈړ����Ȃ��Ȃ�s�������̂łǂ��ɂ�����
    int _prev = -1;

    Vector3 ITargetSelectable.GetNextWaypointPos()
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

    Vector3 ITargetSelectable.GetExitPos() => _exit.position;
}
