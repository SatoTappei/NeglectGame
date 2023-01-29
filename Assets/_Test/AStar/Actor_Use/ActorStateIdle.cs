using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// その場で待機するステートのクラス
/// </summary>
internal class ActorStateIdle : ActorStateBase
{
    internal ActorStateIdle(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }
}
