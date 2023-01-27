using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �L�����N�^�[�̃X�e�[�g�}�V���̊e�X�e�[�g�̊��N���X
/// </summary>
internal abstract class ActorStateBase
{
    protected enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    protected IActorController _actorController;
    protected Stage _stage;
    protected ActorStateBase _nextState;
    protected ActorStateMachine _stateMachine;

    internal ActorStateBase(IActorController actorController, ActorStateMachine stateMachine)
    {
        _actorController = actorController;
        _stateMachine = stateMachine;
        _stage = Stage.Enter;
    }

    internal ActorStateBase Update()
    {
        if      (_stage == Stage.Enter) Enter();
        else if (_stage == Stage.Stay)  Stay();
        else if (_stage == Stage.Exit)
        {
            Exit();
            _stage = Stage.Enter;
            return _nextState;
        }

        return this;
    }

    // �e�X�e�[�g�ŃI�[�o�[���C�h�����ہA���\�b�h�̍Ō�� base. ���ĂԂ���
    protected virtual void Enter() => _stage = Stage.Stay;
    protected virtual void Stay() => _stage = Stage.Stay;
    protected virtual void Exit() => _stage = Stage.Exit;
}

/// <summary>
/// ���̏�őҋ@����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateIdle : ActorStateBase
{
    internal ActorStateIdle(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        Debug.Log("�A�C�h��Enter");
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionAnimationState())
        {

        }
        Debug.Log("�A�C�h��Stay");
        base.Stay();
    }
}

/// <summary>
/// �^�[�Q�b�g�Ɍ������Ĉړ�����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    internal ActorStateMove(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.MoveToTarget(false);
        base.Enter();
    }

    protected override void Stay()
    {
        if(_actorController.IsTransitionMoveState())
        {
            //_nextState = new ActorStateIdle(_actorController);
            Debug.Log("��ԑJ�� to Idle");
            return;
        }

        base.Stay();
    }

    protected override void Exit()
    {
        _actorController.MoveCancel();
        base.Exit();
    }
}

/// <summary>
/// ���낤�낷��X�e�[�g�̃N���X
/// </summary>
internal class ActorStateWander : ActorStateBase
{
    internal ActorStateWander(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }
}

/// <summary>
/// �A�j���[�V�����̍Đ����s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStateAnimation : ActorStateBase
{
    internal ActorStateAnimation(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.PlayAnim();
        Debug.Log("�A�j���Đ�");
        base.Enter();
    }

    protected override void Stay()
    {
        // ������true�Ȃ�
        if (_actorController.IsTransitionIdleState())
        {
            Debug.Log("�A�C�h����");
            _nextState = _stateMachine.GetNextState(StateID.Idle);
            _stage = Stage.Exit;
            return;
        }
        Debug.Log("�ʏ�");
        base.Stay();
    }

    protected override void Exit()
    {
        Debug.Log("Exit����");
        base.Exit();
    }
}

/// <summary>
/// ��������ȏ㓮�����Ȃ���Ԃ̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }
}