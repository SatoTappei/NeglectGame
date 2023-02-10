/// <summary>
/// ��������ȏ㓮�����Ȃ���Ԃ̃X�e�[�g�̃N���X
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