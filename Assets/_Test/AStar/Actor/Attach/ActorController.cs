using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    ActorControlHelper _actorControlHelper = new ActorControlHelper();
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;
    StateID _nextState = StateID.Non;
    /// <summary>
    /// �e�X�e�[�g�ɑJ�ڂ�������false�ɂȂ�A���̃X�e�[�g�̍s�����I�������
    /// true�ɂȂ��ăX�e�[�g����̑J�ډ\�ɂȂ�
    /// </summary>
    bool _isTransitionable;
    /// <summary>���̃t���O��true�ɂȂ����ꍇ�A���͕K��Escape�X�e�[�g�ɑJ�ڂ���</summary>
    bool _isCompleted;

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        // MessageBroker�œG��|�������b�Z�[�W���󂯎��e�X�g�p
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => _isCompleted = true);
    }

    void IStateControl.PlayAnim(StateID current, StateID next)
    {
        string stateName = _actorControlHelper.StateIDToString(current);

        _isTransitionable = false;
        _actorAnimation.PlayAnim(stateName, () => 
        {
            _isTransitionable = true;
            _nextState = next;
        });
    }

    void IStateControl.MoveToRandomWaypoint()
    {
        //Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        //Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        //_isTransitionable = false;
        //_actorAction.MoveFollowPath(pathStack, () => 
        //{
        //    _isTransitionable = true;
        //    _nextState = StateID.LookAround;
        //});
        Hoge(_pathfindingTarget.GetPathfindingTarget(), _actorAction.MoveFollowPath);
        _nextState = StateID.LookAround;
    }

    void IStateControl.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject.gameObject;

        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        //Vector3 targetPos = inSightObject.transform.position;
        //Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        //_isTransitionable = false;
        //_actorAction.RunFollowPath(pathStack, () =>
        //{
        //    _isTransitionable = true;
        //});
        Hoge(inSightObject.transform.position, _actorAction.RunFollowPath);
    }

    void IStateControl.MoveToExit()
    {
        //Vector3 targetPos = _pathfindingTarget.GetExitPos();
        //Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        //_isTransitionable = false;
        //_actorAction.MoveFollowPath(pathStack, () =>
        //{
        //    _isTransitionable = true;
        //    _nextState = StateID.LookAround;
        //});

        _nextState = StateID.LookAround;
        Hoge(_pathfindingTarget.GetExitPos(), _actorAction.MoveFollowPath);
    }

    void Hoge(Vector3 targetPos, UnityAction<Stack<Vector3>, UnityAction> moveMethod)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        moveMethod(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.CancelMoving() => _actorAction.MoveCancel();

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    bool IStateControl.IsTransitionable() => _isTransitionable;

    bool IStateControl.IsDead() => _actorHpControl.IsHpEqualZero();

    bool IStateControl.IsCompleted()
    {
        if (_isCompleted)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    bool IStateControl.IsSightTarget()
    {
        if (_actorSight.IsFindInSight())
        {
            SightableObject inSightObject = _actorSight.CurrentInSightObject;
            
            if (inSightObject.HasWitness()) return false;
            inSightObject.SetWitness(gameObject);

            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.EffectAroundEffectableObject() => _actorEffecter.EffectAround();
}