using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットに向かって走って移動するステートのクラス
/// </summary>
internal class ActorStateRun : ActorStateMove
{
    internal ActorStateRun(IStateControl movable, ActorStateMachine stateMachine)
    : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.RunToTarget();
    }

    protected override void Exit()
    {
        _stateControl.RunEndable();
    }
}
