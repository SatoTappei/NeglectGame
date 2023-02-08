/// <summary>
/// ターゲットに向かって走って移動するステートのクラス
/// </summary>
internal class ActorStateRun : ActorStateBase
{
    internal ActorStateRun(ActorStateMachine stateMachine)
    : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.RunToTarget();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() &&
            _stateMachine.StateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateID.Attack))
        {
            ChangeState(StateID.Attack);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateID.Joy))
        {
            ChangeState(StateID.Joy);
        }
    }

    protected override void Exit()
    {
        _stateMachine.StateControl.CancelMoveToTarget();
    }
}
