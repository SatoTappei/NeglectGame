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

        // �ړI�n�ɓ��������猩�񂷃A�j���[�V�����̒����������҂��Ƃ�
        // �A�j���[�V�����̏I����ҋ@���Ă̏������������Ă���
        if (_stateMachine.StateControl.IsArrivalWaypoint())
        {
            _isArraival = true;

            _stateMachine.StateControl.PlayAnimation("LookAround");

            float delayTime = _stateMachine.StateControl.GetAnimationClipLength("LookAround");
            _tween = DOVirtual.DelayedCall(delayTime, () =>
            {
                ChangeState(StateType.Explore);
            }).SetLink(_stateMachine.gameObject);

            return;
        }

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        if (inSightObject != null)
        {
            ChangeState(StateType.MoveToInSight);
            return;
        }

        // ���C�����ȉ��̎�
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
