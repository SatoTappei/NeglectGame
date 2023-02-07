/// <summary>
/// 周囲を見渡すアニメーションをするステートのクラス
/// </summary>
internal class ActorStateLookAround : ActorStateBase
{
    internal ActorStateLookAround(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim(StateID.LookAround, StateID.Move);
    }

    protected override void Stay()
    {
        if (_stateControl.IsDead() && _stateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Move))
        {
            ChangeState(StateID.Move);
        }
    }
}