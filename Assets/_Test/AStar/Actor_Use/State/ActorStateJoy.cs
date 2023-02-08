/// <summary>
/// �ڕW��B�������ۂɊ�ԃA�j���[�V�������s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStateJoy : ActorStateBase
{
    internal ActorStateJoy(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Joy, StateID.Escape);
        _stateMachine.StateControl.EffectAround();
    }

    protected override void Stay()
    {
        // �ڕW��B��������A�҃X�e�[�g�ɑJ�ڂ���悤�ɍ��
        if (_stateMachine.StateControl.IsTransitionable() && 
            _stateMachine.StateControl.IsEqualNextState(StateID.Escape))
        {
            ChangeState(StateID.Escape);
        }
    }
}
