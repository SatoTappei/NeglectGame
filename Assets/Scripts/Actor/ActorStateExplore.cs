using DG.Tweening;

/// <summary>
/// ActorStateMachine�Ŏg�p���郉���_����Waypoint�Ɍ����Ĉړ�����X�e�[�g�̃N���X
/// </summary>
public class ActorStateExplore : ActorStateBase
{
    bool _isArraival;
    Tween _tween;

    public ActorStateExplore(ActorStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToWaypoint();
    }

    protected override void Stay()
    {
        if (_isArraival) return;

        // �ړI�n�ɓ��������猩�񂷃A�j���[�V�������Đ�������ɑJ�ڂ���
        if (_stateMachine.StateControl.IsArrivalWaypoint())
        {
            _isArraival = true;

            _stateMachine.StateControl.PlayAnimation("LookAround");

            float delayTime = _stateMachine.StateControl.GetAnimationClipLength("LookAround");
            _tween = DOVirtual.DelayedCall(delayTime, () =>
            {
                ChangeState(StateType.Explore);
            }).SetLink(_stateMachine.gameObject);
        }

        // ���C�����ȉ��̎�

        // �󔠂��������Ƃ�

        // �G�𔭌������Ƃ�
        //  n%�̊m���Ō��ʂ�����/�����̃X�e�[�g�ɑJ��
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
