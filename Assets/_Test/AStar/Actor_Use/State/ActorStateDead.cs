/// <summary>
/// ��������ȏ㓮�����Ȃ���Ԃ̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim("Dead");
    }
}