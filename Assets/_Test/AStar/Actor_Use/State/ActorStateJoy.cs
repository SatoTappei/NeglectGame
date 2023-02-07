/// <summary>
/// 目標を達成した際に喜ぶアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateJoy : ActorStateBase
{
    internal ActorStateJoy(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim(StateID.Joy, StateID.Non);
    }

    protected override void Stay()
    {
        // 目標を達成したら帰還ステートに遷移するように作る
        if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Escape))
        {
            ChangeState(StateID.Escape);
        }
    }
}
