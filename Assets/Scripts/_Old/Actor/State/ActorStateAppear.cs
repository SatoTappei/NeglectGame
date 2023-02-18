/// <summary>
/// 登場時のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateAppear : ActorStateBaseOld
{
    internal ActorStateAppear(ActorStateMachineOld stateMachine)
            : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Appear, StateIDOld.Move);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsTransitionable() &&
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Move))
        {
            ChangeState(StateIDOld.Move);
        }
    }
}