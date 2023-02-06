/// <summary>
/// ターゲットに向かって走って移動するステートのクラス
/// </summary>
internal class ActorStateRun : ActorStateBase
{
    internal ActorStateRun(IStateControl stateControl, ActorStateMachine stateMachine)
    : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.RunToTarget();
    }

    protected override void Stay()
    {
        if (_stateControl.IsDead() && _stateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Attack))
        {
            ChangeState(StateID.Attack);
        }
        else if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Joy))
        {
            ChangeState(StateID.Joy);
        }
    }

    protected override void Exit()
    {
        _stateControl.CancelMoveToTarget();
    }
}
