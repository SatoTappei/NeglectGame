/// <summary>
/// �퓬�Ŕs�k���Ď��S�����ۂ̃X�e�[�g�̃N���X
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
