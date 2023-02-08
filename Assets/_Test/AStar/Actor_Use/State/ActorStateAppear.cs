/// <summary>
/// 登場時のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateAppear : ActorStateBase
{
    internal ActorStateAppear(ActorStateMachine stateMachine)
            : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Appear, StateID.Move);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsTransitionable() &&
            _stateMachine.StateControl.IsEqualNextState(StateID.Move))
        {
            ChangeState(StateID.Move);
        }
    }
}