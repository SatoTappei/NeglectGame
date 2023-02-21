using DG.Tweening;

/// <summary>
/// �����̓����܂ňړ�����X�e�[�g�̃N���X
/// </summary>
public class ActorStateMoveToRoom : ActorStateBase
{
    bool _isArraival;
    Tween _tween;

    public ActorStateMoveToRoom(ActorStateMachine stateMachine) : base(stateMachine)
    {

    }

    protected override void Enter()
    {
        // ���̃X�e�[�g�ɑJ�ڂ���O�Ɏ��E�̋@�\��؂��Ă���̂ŁA�O�t���[�����王�E���X�V���ꂸ
        // RoomEntrance��ނ̃I�u�W�F�N�g���擾�ł���
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        _stateMachine.StateControl.MoveTo(inSightObject.gameObject.transform.position);

        // ���E�ɂƂ炦�����̂ɉ�����Sequence�����s�������̂ōēx�����Ŏ��E�̋@�\���I���ɂ��Ă���
        _stateMachine.StateControl.ToggleSight(isActive: true);
    }

    protected override void Stay()
    {
        // �����̓������������ꍇ�ɑJ�ڂ��s���Ɩ������[�v�Ɋׂ�\��������̂Œe��
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        if (inSightObject?.SightableType == SightableType.Treasure ||
            inSightObject?.SightableType == SightableType.Enemy)
        {
            ChangeState(StateType.InSightSelect);
            return;
        }

        if (_isArraival) return;

        // �ړI�n�ɓ��������猩�񂷃A�j���[�V�����̒����������҂��Ƃ�
        // �A�j���[�V�����̏I����ҋ@���Ă̏������������Ă���
        if (_stateMachine.StateControl.IsArrivalTargetPos())
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

        // ���C�����ȉ��̎�
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
