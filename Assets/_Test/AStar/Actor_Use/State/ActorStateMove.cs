/// <summary>
/// ターゲットに向かって移動するステートのクラス
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    internal ActorStateMove(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToTarget();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() && 
            _stateMachine.StateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateMachine.StateControl.IsSightTarget() && 
                 _stateMachine.StateControl.IsEqualNextState(StateID.Panic))
        {
            ChangeState(StateID.Panic);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateID.LookAround))
        {
            ChangeState(StateID.LookAround);
        }
    }

    protected override void Exit()
    {
        _stateMachine.StateControl.CancelMoveToTarget();
    }
}