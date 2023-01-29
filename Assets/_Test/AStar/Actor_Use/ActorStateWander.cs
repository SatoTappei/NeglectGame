using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// うろうろするステートのクラス
/// </summary>
internal class ActorStateWander : ActorStateBase
{
    internal ActorStateWander(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.PlayWanderAnim();
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
            return;
        }

        if (_actorController.IsTransitionable())
        {
            ChangeState(StateID.Move);
            return;
        }
        base.Stay();
    }

    protected override void Exit()
    {
        base.Exit();
    }
}