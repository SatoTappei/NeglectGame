using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorAction _actorAction;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    /// <summary>
    /// �e�X�e�[�g�ɑJ�ڂ�������false�ɂȂ�
    /// ���̃X�e�[�g�̍s�����I�������true�ɂȂ��ăX�e�[�g����̑J�ډ\�ɂȂ�
    /// </summary>
    bool _isTransitionable;

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();
    }

    public void MoveToTarget()
    {
        _isTransitionable = false;
        _actorAction.MoveFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void RunToTarget()
    {
        _isTransitionable = false;
        _actorAction.RunFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void CancelMoveToTarget() => _actorAction.MoveCancel();

    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    public bool IsTransitionable() => _isTransitionable;

    public void PlayWanderAnim()
    {
        _isTransitionable = false;
        _actorAction.LookAround(() => _isTransitionable = true);
    }

    public void PlayAppearAnim()
    {
        _isTransitionable = false;
        _actorAction.PlayAppearAnim(() => _isTransitionable = true);
    }

    public bool IsTransitionToPanicState()
    {
        // ������������
        return Input.GetKeyDown(KeyCode.W);
    }

    public void PlayPanicAnim()
    {
        _isTransitionable = false;
        _actorAction.PlayPanicAnim(() => _isTransitionable = true);
    }

    public bool IsTransitionToDeadState()
    {
        // �����S�𔻒肷��
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Debug.Log("���񂾂Ƃ��ɂȂ񂩉��o������");
    }
}
