using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorAction _actorAction;
    [SerializeField] ActorHpControl _actorStatus;
    [SerializeField] ActorSight _actorSight;

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

        ActorStatus actorStatus = new ActorStatus();
    }

    public bool IsTransitionable() => _isTransitionable;

    public void MoveToTarget() => WaitUntilArrival(_actorAction.MoveFollowPath);
    //public void RunToTarget() => WaitUntilArrival(_actorAction.RunFollowPath);
    public void RunToTarget()
    {

    }
    public void CancelMoveToTarget() => _actorAction.MoveCancel();

    void WaitUntilArrival(UnityAction<Stack<Vector3>, UnityAction> unityAction)
    {
        _isTransitionable = false;
        unityAction(GetPathStack(), () => _isTransitionable = true);
    }

    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    public void PlayWanderAnim() => WaitAnimFinish(_actorAction.PlayLookAroundAnim);
    public void PlayAppearAnim() => WaitAnimFinish(_actorAction.PlayAppearAnim);
    public void PlayPanicAnim() => WaitAnimFinish(_actorAction.PlayPanicAnim);

    void WaitAnimFinish(UnityAction<UnityAction> unityAction)
    {
        _isTransitionable = false;
        unityAction(() => _isTransitionable = true);
    }

    /* 
     *  TODO:��x�������������ɂ�����x�����������ƃ��[�v���Ă��܂��s�
     *       ��x�����������ƑΏۂ̂������ɓ����܂Ŕ������Ȃ��悤�ɂ���
     *       �t���O�̃I���I�t�Ƃ��H
     */

    /*  �����������ǂ�����bool���Ԃ�
     *  Panic�X�e�[�g��Run�X�e�[�g�����S�ɕ������Ă���
     *  Panic�X�e�[�g�̎���Run�X�e�[�g�ł͌���������Ɍ����đ����Ă����悤�ɂ�����
     *  ����̍��W��n���΂��̍��W�܂Ōo�H�T�����ĕ����Ă����
     *  1.���󔭌�
     *  2.Panic�X�e�[�g�ɑJ��
     *  3.�����_���ȉӏ���I�� <= ������ς�����
     *  4.Run�X�e�[�g�ł��̍��W�Ɍ������đ���
     */
    public bool IsTransitionToPanicState() => _actorSight.IsFindTreasure();
    public bool IsTransitionToDeadState() => _actorStatus.IsHpIsZero();

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Debug.Log("���񂾂Ƃ��ɂȂ񂩉��o������");
    }
}