/// <summary>
/// 戦闘で敗北して死亡した際のステートのクラス
/// </summary>
public class ActorStateDead : ActorStateBase
{
    public ActorStateDead(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayDeadEffect();
    }
}
