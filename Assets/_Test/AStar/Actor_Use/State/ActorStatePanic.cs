/// <summary>
/// 目標を発見した時のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStatePanic : ActorStateBase
{
    internal ActorStatePanic(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Panic, StateID.Run);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsTransitionable() &&
            _stateMachine.StateControl.IsEqualNextState(StateID.Run))
        {
            ChangeState(StateID.Run);
        }
    }
}