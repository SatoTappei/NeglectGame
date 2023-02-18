/// <summary>
/// 目標を発見した時のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStatePanic : ActorStateBaseOld
{
    internal ActorStatePanic(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Panic, StateIDOld.Run);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsTransitionable() &&
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Run))
        {
            ChangeState(StateIDOld.Run);
        }
    }
}