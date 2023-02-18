/// <summary>
/// �ڕW��B�������ۂɊ�ԃA�j���[�V�������s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStateJoy : ActorStateBaseOld
{
    internal ActorStateJoy(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Joy, StateIDOld.Escape);
        _stateMachine.StateControl.EffectAroundEffectableObject();
    }

    protected override void Stay()
    {
        // �ڕW��B��������A�҃X�e�[�g�ɑJ�ڂ���悤�ɍ��
        if (_stateMachine.StateControl.IsTransitionable() && 
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Escape))
        {
            ChangeState(StateIDOld.Escape);
        }
    }
}
