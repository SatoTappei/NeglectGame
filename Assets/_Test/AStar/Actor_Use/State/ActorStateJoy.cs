using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// 目標を達成した際の喜びのステートのクラス
/// </summary>
internal class ActorStateJoy : ActorStateBase
{
    internal ActorStateJoy(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAppearAnim();
    }
}
