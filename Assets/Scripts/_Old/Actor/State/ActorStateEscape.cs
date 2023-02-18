/// <summary>
/// 目標を達成し、返る際のステートのクラス
/// </summary>
internal class ActorStateEscape : ActorStateBaseOld
{
    internal ActorStateEscape(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToExit();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() && 
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Dead))
        {
            ChangeState(StateIDOld.Dead);
        }

        // TODO: else if 階段に到着したら脱出する処理
    }

    protected override void Exit()
    {
        _stateMachine.StateControl.CancelMoving();
    }
}
