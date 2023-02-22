using DG.Tweening;

/// <summary>
/// �����_����Waypoint�Ɍ����Ĉړ�����X�e�[�g�̃N���X
/// </summary>
public class ActorStateExplore : ActorStateBase
{
    bool _isArraival;
    Tween _tween;

    public ActorStateExplore(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToWaypoint();
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.GetInSightAvailableMovingTarget() != null)
        {
            _tween?.Kill();
            ChangeState(StateType.InSightSelect);
            return;
        }

        // �ړI�n�ɓ��������猩�񂷃A�j���[�V�����̒����������҂��Ƃ�
        // �A�j���[�V�����̏I����ҋ@���Ă̏������������Ă���
        if (_stateMachine.StateControl.IsTargetPosArrival() && !_isArraival)
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
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
