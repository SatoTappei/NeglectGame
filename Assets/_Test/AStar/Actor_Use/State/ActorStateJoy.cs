/// <summary>
/// 目標を達成した際に喜ぶアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateJoy : ActorStateBase
{
    internal ActorStateJoy(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Joy, StateID.Escape);
        _stateMachine.StateControl.EffectAround();
    }

    protected override void Stay()
    {
        // 目標を達成したら帰還ステートに遷移するように作る
        if (_stateMachine.StateControl.IsTransitionable() && 
            _stateMachine.StateControl.IsEqualNextState(StateID.Escape))
        {
            ChangeState(StateID.Escape);
        }
    }
}
