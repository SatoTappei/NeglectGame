/// <summary>
/// �ڕW�𔭌��������̃A�j���[�V�������s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStatePanic : ActorStateBase
{
    internal ActorStatePanic(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Panic, StateID.Run);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsTransitionable() &&
            _stateMachine.StateControl.IsEqualNextState(StateID.Run))
        {
            ChangeState(StateID.Run);
        }
    }
}