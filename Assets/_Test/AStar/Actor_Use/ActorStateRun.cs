using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ターゲットに向かって走って移動するステートのクラス
/// </summary>
internal class ActorStateRun : ActorStateMove
{
    internal ActorStateRun(IActorController movable, ActorStateMachine stateMachine)
    : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.RunToTarget();
        // .baseを呼ぶとMoveToTarget()が実行されてしまうので応急処置的にここでStageを変更する
        _stage = Stage.Stay;
    }
}
