using DG.Tweening;

/// <summary>
/// ActorStateMachine�Ŏg�p���錩�����ΏۂɌ����Ĉړ�����X�e�[�g�̃N���X
/// </summary>
public class ActorStateMoveToInSight : ActorStateBase
{
    bool _isArraival;
    Tween _tween;

    public ActorStateMoveToInSight(ActorStateMachine stateMachine) : base(stateMachine)
    {

    }

    protected override void Enter()
    {
        _stateMachine.StateControl.MoveToInSightObject();
        Debug.Log("����������");
    }

    protected override void Stay()
    {
        //if (_isArraival) return;

        //if (_stateMachine.StateControl.IsArrivalWaypoint())
        //{
        //    _isArraival = true;

        //    _stateMachine.StateControl.PlayAnimation("LookAround");

        //    float delayTime = _stateMachine.StateControl.GetAnimationClipLength("LookAround");
        //    _tween = DOVirtual.DelayedCall(delayTime, () =>
        //    {
        //        ChangeState(StateType.Explore);
        //    }).SetLink(_stateMachine.gameObject);

        //    return;
        //}

        //SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        //if (inSightObject?.SightableType == SightableType.Treasure)
        //{
        //    //  �󔠂�������
        //    // Sequence�̎��s
        //}
        //else if (inSightObject?.SightableType == SightableType.Enemy)
        //{
        //    //  �G�𔭌�����
        //    //      n%�̊m���Ō��ʂ�����/�����̃X�e�[�g�ɑJ��
        //    // Sequence�̎��s
        //}

        //if (inSightObject?.SightableType == SightableType.Waypoint)
        //{
        //    //  �����𔭌�����
        //    // �����̒��֓���
        //    // ���낤��֖߂�
        //    // �X�e�[�g���؂�ւ��͎̂��̃t���[������
        //    // �����Ăяo�����̊֌W�ł��̃t���[������InSightObject���ς���Ă��܂�����
        //    // ������Waypoint�Ɍ����������̂͂������G�Ɍ������ĕ����čs���Ă��܂��B

        //    // �Ώ�:�����𔭌������甭���������̂Ɍ����ĕ����Ă����X�e�[�g�ɑJ�ڂ���
        //    //      ���̃X�e�[�g���Ŕ����������s��
        //    //      ��/�G�𔭌�������eSequence�ɑJ�ڂ���

        //    return;
        //}
        //else if(inSightObject?.SightableType == SightableType.Treasure)
        //{
        //    //  �󔠂�������
        //}
        //else if (inSightObject?.SightableType == SightableType.Enemy)
        //{
        //    //  �G�𔭌�����
        //    //      n%�̊m���Ō��ʂ�����/�����̃X�e�[�g�ɑJ��
        //}
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
