using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] ActorMove _pathfindingMove;
    [Header("System�I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    void Start()
    {
        // TODO:���̎Q�Ɛ�̎擾���@���Ȃ񂩋C�ɂȂ�
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();
    }

    public void MoveToTarget()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _pathfindingMove.MoveFollowPath(pathStack);
    }

    public void MoveCancel()
    {
        throw new System.NotImplementedException();
    }

    public void PlayAnim()
    {
        throw new System.NotImplementedException();
    }
}
