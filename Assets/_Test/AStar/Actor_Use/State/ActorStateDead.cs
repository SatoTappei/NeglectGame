/// <summary>
/// もうこれ以上動かさない状態のステートのクラス
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Dead, StateID.Non);
    }
}