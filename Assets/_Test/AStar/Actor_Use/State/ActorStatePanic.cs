/// <summary>
/// �ڕW�𔭌��������̃A�j���[�V�������s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStatePanic : ActorStateBase
{
    internal ActorStatePanic(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim(StateID.Panic, StateID.Run);
    }

    protected override void Stay()
    {
        if ( _stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Run))
        {
            ChangeState(StateID.Run);
        }
    }
}