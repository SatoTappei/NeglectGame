/// <summary>
/// 戦闘で敗北して死亡した際のステートのクラス
/// </summary>
public class ActorStateDead : ActorStateBase
{
    public ActorStateDead(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayDeadPerformance();
    }
}
