/// <summary>
/// 周囲を見渡すアニメーションをするステートのクラス
/// </summary>
internal class ActorStateLookAround : ActorStateBase
{
    internal ActorStateLookAround(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.LookAround, StateID.Move);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() && 
            _stateMachine.StateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateID.Move))
        {
            ChangeState(StateID.Move);
        }
    }
}