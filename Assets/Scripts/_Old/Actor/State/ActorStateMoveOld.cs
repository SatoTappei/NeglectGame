/// <summary>
/// ターゲットに向かって移動するステートのクラス
/// </summary>
internal class ActorStateMoveOld : ActorStateBaseOld
{
    internal ActorStateMoveOld(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToRandomWaypoint();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() && 
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Dead))
        {
            ChangeState(StateIDOld.Dead);
        }
        else if (_stateMachine.StateControl.IsSightTarget() && 
                 _stateMachine.StateControl.IsEqualNextState(StateIDOld.Panic))
        {
            ChangeState(StateIDOld.Panic);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateIDOld.LookAround))
        {
            ChangeState(StateIDOld.LookAround);
        }
    }

    protected override void Exit()
    {
        _stateMachine.StateControl.CancelMoving();
    }
}