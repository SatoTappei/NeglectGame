using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// うろうろするステートのクラス
/// </summary>
internal class ActorStateLookAround : ActorStateBase
{
    internal ActorStateLookAround(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayLookAroundAnim();
    }

    protected override void Stay()
    {
        if (_stateControl.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateControl.IsTransitionable())
        {
            ChangeState(StateID.Move);
        }
    }
}