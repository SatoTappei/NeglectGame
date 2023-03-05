using DG.Tweening;

/// <summary>
/// �o�ꎞ�̉��o���s���X�e�[�g�̃N���X
/// </summary>
public class ActorStateEntry : ActorStateBase
{
    Tween _tween;

    public ActorStateEntry(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnimation("Entry");

        // �A�j���[�V�����̒���������Delay���邱�ƂŁA�A�j���[�V�����ɍ��킹���J�ڂɌ�����
        float delayTime = _stateMachine.StateControl.GetAnimationClipLength("Entry");
        _tween = DOVirtual.DelayedCall(delayTime, () =>
        {
            ChangeState(StateType.Explore);
        }).SetLink(_stateMachine.gameObject);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsHpEqualZero())
        {
            TryChangeState(StateType.Dead);
            return;
        }
    }

    protected override void Exit()
    {
        _tween?.Kill();
    }

    public override void OnStateMachinePause()
    {
        _tween?.Kill();
    }
}
