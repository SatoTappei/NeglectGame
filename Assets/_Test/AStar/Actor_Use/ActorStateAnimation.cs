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
        Debug.Log("アニメ再生");
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

        Debug.Log("通常");
        base.Stay();
    }

    protected override void Exit()
    {
        Debug.Log("Exit処理");
        base.Exit();
    }
}
