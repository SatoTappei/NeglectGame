using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステートマシンの各ステートの基底クラス
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
/// その場で待機するステートのクラス
/// </summary>
internal class ActorStateIdle : ActorStateBase
{
    internal ActorStateIdle(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Stay()
    {
        if (_actorController.IsTransionAnimationState())
        {
            // アニメーションに遷移
        }

        base.Stay();
    }
}

/// <summary>
/// ターゲットに向かって移動するステートのクラス
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
            Debug.Log("状態遷移 to Idle");
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
/// うろうろするステートのクラス
/// </summary>
internal class ActorStateWander : ActorStateBase
{
    internal ActorStateWander(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }
}

/// <summary>
/// アニメーションの再生を行うステートのクラス
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
/// もうこれ以上動かさない状態のステートのクラス
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }
}