using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// もうこれ以上動かさない状態のステートのクラス
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayDeadAnim();
    }
}