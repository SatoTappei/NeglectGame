/// <summary>
/// �퓬�Ŕs�k���Ď��S�����ۂ̃X�e�[�g�̃N���X
/// </summary>
public class ActorStateDead : ActorStateBase
{
    public ActorStateDead(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayDeadEffect();
    }
}
