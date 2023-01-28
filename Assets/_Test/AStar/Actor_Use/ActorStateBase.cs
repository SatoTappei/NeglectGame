using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

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

    // 各ステートでオーバーライドした際、メソッドの"最後"で base. を呼ぶこと
    protected virtual void Enter() => _stage = Stage.Stay;
    protected virtual void Stay() => _stage = Stage.Stay;
    protected virtual void Exit() => _stage = Stage.Exit;
}

/// <summary>
/// その場で待機するステートのクラス
/// </summary>
internal class ActorStateIdle : ActorStateBase
{
    internal ActorStateIdle(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        Debug.Log("アイドルEnter");
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionAnimationState())
        {

        }
        Debug.Log("アイドルStay");
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
        Debug.Log("移動開始");
        _actorController.MoveToTarget(true);
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState())
        {
            _nextState = _stateMachine.GetNextState(StateID.Dead);
            _stage = Stage.Exit;
            return;
        }

        if (_actorController.IsTransitionToAnimationStateFromMoveState())
        {
            // なんか発見したときのアニメーションを再生
            _nextState = _stateMachine.GetNextState(StateID.Discover);
            _stage = Stage.Exit;
            return;
        }

        if (_actorController.IsTransitionToWanderStateFromMoveState())
        {
            // うろうろに遷移処理をかっくところかｒ
            _nextState = _stateMachine.GetNextState(StateID.Wander);
            _stage = Stage.Exit;
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

    protected override void Enter()
    {
        _actorController.PlayLookAround();
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState())
        {
            _nextState = _stateMachine.GetNextState(StateID.Dead);
            _stage = Stage.Exit;
            return;
        }

        if (_actorController.IsTransitionToMoveStateFromWanderStateAfterLookAroundDOtweenAnimation())
        {
            _nextState = _stateMachine.GetNextState(StateID.Move);
            _stage = Stage.Exit;
            return;
        }
        base.Stay();
    }

    protected override void Exit()
    {
        base.Exit();
    }
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
        Debug.Log("アニメ再生");
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState())
        {
            _nextState = _stateMachine.GetNextState(StateID.Dead);
            _stage = Stage.Exit;
            return;
        }

        if (_actorController.IsTransitionIdleState())
        {
            _nextState = _stateMachine.GetNextState(StateID.Move);
            _stage = Stage.Exit;
            return;
        }

        Debug.Log("通常");
        base.Stay();
    }

    protected override void Exit()
    {
        Debug.Log("Exit処理");
        base.Exit();
    }
}

/// <summary>
/// 発見した時のステートのクラス
/// </summary>
internal class ActorStateDiscover : ActorStateBase
{
    internal ActorStateDiscover(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        Debug.Log("八犬伝");
        _actorController.PlayDiscoverAnim();
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionToMoveStateFromDiscoverState())
        {
            _nextState = _stateMachine.GetNextState(StateID.Move);
            _stage = Stage.Exit;
            return;
        }
        base.Stay();
    }

    protected override void Exit()
    {
        base.Exit();
    }
}

/// <summary>
/// もうこれ以上動かさない状態のステートのクラス
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.FromAnyStateDead();
        // これ以上処理をしないので .base は呼ばない
    }
}