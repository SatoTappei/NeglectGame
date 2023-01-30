using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// アニメーションの再生を行うステートのクラス
/// </summary>
internal class ActorStateAnimation : ActorStateBase
{
    internal ActorStateAnimation(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.PlayAppearAnim();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
        }
        else if (_actorController.IsTransitionable())
        {
            ChangeState(StateID.Move);
        }
    }
}
