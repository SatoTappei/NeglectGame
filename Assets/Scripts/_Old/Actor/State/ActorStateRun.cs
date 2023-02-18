/// <summary>
/// ターゲットに向かって走って移動するステートのクラス
/// </summary>
internal class ActorStateRun : ActorStateBaseOld
{
    internal ActorStateRun(ActorStateMachineOld stateMachine)
    : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.RunToTarget();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() &&
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Dead))
        {
            ChangeState(StateIDOld.Dead);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateIDOld.Attack))
        {
            ChangeState(StateIDOld.Attack);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateIDOld.Joy))
        {
            ChangeState(StateIDOld.Joy);
        }
    }

    protected override void Exit()
    {
        _stateMachine.StateControl.CancelMoving();
    }
}
