using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// ターゲットに向かって移動するステートのクラス
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    internal ActorStateMove(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.MoveToTarget();
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
            return;
        }

        if (_actorController.IsTransitionToPanicState())
        {
            ChangeState(StateID.Panic);
            return;
        }

        if (_actorController.IsTransitionable())
        {
            ChangeState(StateID.Wander);
            return;
        }

        base.Stay();
    }

    protected override void Exit()
    {
        _actorController.CancelMoveToTarget();
        base.Exit();
    }
}