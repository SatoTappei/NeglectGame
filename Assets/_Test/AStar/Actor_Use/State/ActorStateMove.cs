using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// ターゲットに向かって移動するステートのクラス
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    readonly float Interval = 0.3f;
    float _timer;

    internal ActorStateMove(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _timer = 0;
        _stateControl.MoveToTarget();
    }

    protected override void Stay()
    {
        _timer += Time.deltaTime;

        if (_stateControl.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
        }
        // ActorControllerで実装している処理が重いので毎フレーム呼ばないようにしている
        else if (_timer > Interval)
        {
            _timer = 0;
            if (_stateControl.IsTransitionToPanicState())
            {
                ChangeState(StateID.Panic);
            }
        }
        else if (_stateControl.IsTransitionable())
        {
            ChangeState(StateID.LookAround);
        }
    }

    protected override void Exit()
    {
        _stateControl.CancelMoveToTarget();
    }
}