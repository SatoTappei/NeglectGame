/// <summary>
/// �o�ꎞ�̃A�j���[�V�������s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStateAppear : ActorStateBase
{
    internal ActorStateAppear(IStateControl stateControl, ActorStateMachine stateMachine)
            : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim(StateID.Appear, StateID.Move);
    }

    protected override void Stay()
    {
        if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Move))
        {
            ChangeState(StateID.Move);
        }
    }
}