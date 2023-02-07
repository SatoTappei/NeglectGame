/// <summary>
/// �ڕW��B�������ۂɊ�ԃA�j���[�V�������s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStateJoy : ActorStateBase
{
    internal ActorStateJoy(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim(StateID.Joy, StateID.Non);
    }

    protected override void Stay()
    {
        // �ڕW��B��������A�҃X�e�[�g�ɑJ�ڂ���悤�ɍ��
        if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Escape))
        {
            ChangeState(StateID.Escape);
        }
    }
}
