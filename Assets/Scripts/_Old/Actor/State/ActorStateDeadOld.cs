/// <summary>
/// ��������ȏ㓮�����Ȃ���Ԃ̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateDeadOld : ActorStateBaseOld
{
    internal ActorStateDeadOld(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Dead, StateIDOld.Non);
        //Debug.Log(_stateMachine.gameObject.name + ": ���S");
    }
}