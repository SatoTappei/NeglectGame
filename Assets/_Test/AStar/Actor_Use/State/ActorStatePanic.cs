using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// 発見した時のステートのクラス
/// </summary>
internal class ActorStatePanic : ActorStateBase
{
    internal ActorStatePanic(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayPanicAnim();
    }

    protected override void Stay()
    {
        if (_stateControl.IsTransitionable())
        {
            ChangeState(StateID.Run);
        }
    }
}