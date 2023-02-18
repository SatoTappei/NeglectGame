/// <summary>
/// 目標を達成した際に喜ぶアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateJoy : ActorStateBaseOld
{
    internal ActorStateJoy(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Joy, StateIDOld.Escape);
        _stateMachine.StateControl.EffectAroundEffectableObject();
    }

    protected override void Stay()
    {
        // 目標を達成したら帰還ステートに遷移するように作る
        if (_stateMachine.StateControl.IsTransitionable() && 
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Escape))
        {
            ChangeState(StateIDOld.Escape);
        }
    }
}
