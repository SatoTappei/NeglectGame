/// <summary>
/// �ڕW��B�����A�Ԃ�ۂ̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateEscape : ActorStateBase
{
    internal ActorStateEscape(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToExit();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() && 
            _stateMachine.StateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }

        // TODO: else if �K�i�ɓ���������E�o���鏈��
    }

    protected override void Exit()
    {
        _stateMachine.StateControl.CancelMoving();
    }
}
