/// <summary>
/// 目標を達成し、返る際のステートのクラス
/// </summary>
internal class ActorStateEscape : ActorStateBase
{
    internal ActorStateEscape(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.MoveToExit();
    }

    protected override void Stay()
    {
        if (_stateControl.IsDead() && _stateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }

        // TODO: else if 階段に到着したら脱出する処理
    }

    protected override void Exit()
    {
        _stateControl.CancelMoveToTarget();
    }
}
