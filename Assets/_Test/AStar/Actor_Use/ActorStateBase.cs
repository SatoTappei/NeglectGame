using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected virtual void Enter() => _stage = Stage.Stay;
    protected virtual void Stay() => _stage = Stage.Stay;
    protected virtual void Exit() => _stage = Stage.Exit;

    internal ActorStateBase Update()
    {
        if      (_stage == Stage.Enter) Enter();
        else if (_stage == Stage.Stay)  Stay();
        else if (_stage == Stage.Exit)
        {
            Exit();
            return _nextState;
        }

        return this;
    }
}

/// <summary>
/// ���̏�őҋ@����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateIdle : ActorStateBase
{
    internal ActorStateIdle(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Stay()
    {
        if (_actorController.IsTransionAnimationState())
        {
            // �A�j���[�V�����ɑJ��
        }

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
        if(_actorController.IsTransionMoveState())
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
        base.Enter();
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