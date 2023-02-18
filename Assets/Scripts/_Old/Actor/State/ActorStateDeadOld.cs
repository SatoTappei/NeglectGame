/// <summary>
/// もうこれ以上動かさない状態のステートのクラス
/// </summary>
internal class ActorStateDeadOld : ActorStateBaseOld
{
    internal ActorStateDeadOld(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Dead, StateIDOld.Non);
        //Debug.Log(_stateMachine.gameObject.name + ": 死亡");
    }
}