using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ړ�����L�����N�^�[��MVP�Ŏ�������ׂ�Presenter
/// </summary>
public class PathfindingPresenter : MonoBehaviour, IMovable
{
    [SerializeField] PathfindingMove _pathfindingMove;
    [Header("IPathGetable�̃I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;

    IPathGetable _pathGetable;

    void Start()
    {
        //_pathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
    }

    public void MoveStart(Vector3 targetPos)
    {
        // �������Ŏ擾�͈ꎞ�I�ȏ��u�A����
        _pathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
        MoveToTarget(targetPos);
    }

    void MoveToTarget(Vector3 targetPos)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _pathfindingMove.MoveFollowPath(pathStack);
    }
}
