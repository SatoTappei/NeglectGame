using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// 攻撃するステートのクラス
/// </summary>
internal class ActorStateAttack : ActorStateBase
{
    internal ActorStateAttack(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAttackAnim();
    }
}
