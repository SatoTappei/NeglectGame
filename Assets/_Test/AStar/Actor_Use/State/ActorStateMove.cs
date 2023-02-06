/// <summary>
/// ターゲットに向かって移動するステートのクラス
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    internal ActorStateMove(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.MoveToTarget();
    }

    protected override void Stay()
    {
        if (_stateControl.IsDead() && _stateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateControl.IsSightTarget() && _stateControl.IsEqualNextState(StateID.Panic))
        {
            ChangeState(StateID.Panic);
        }
        else if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.LookAround))
        {
            ChangeState(StateID.LookAround);
        }
    }

    protected override void Exit()
    {
        _stateControl.CancelMoveToTarget();
    }
}