/// <summary>
/// �ڕW��B�����A�Ԃ�ۂ̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateEscape : ActorStateBase
{
    internal ActorStateEscape(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.MoveToExit();
    }

    protected override void Stay()
    {
        if (_stateControl.IsDead() && _stateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }

        // TODO: else if �K�i�ɓ���������E�o���鏈��
    }

    protected override void Exit()
    {
        _stateControl.CancelMoveToTarget();
    }
}
