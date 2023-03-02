using DG.Tweening;

/// <summary>
/// �����̓����܂ňړ�����X�e�[�g�̃N���X
/// </summary>
public class ActorStateEnterTheRoom : ActorStateBase
{
    bool _isArraival;
    Tween _tween;

    public ActorStateEnterTheRoom(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    protected override void Enter()
    {
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        if (inSightObject.SightableType != SightableType.RoomEntrance)
        {
            Debug.LogWarning("RoomEntrance��ނ̃I�u�W�F�N�g���擾�ł��܂���ł����B");
            return;
        }

        _stateMachine.StateControl.RunTo(inSightObject);
    }

    protected override void Stay()
    {
        // �����̓������������ꍇ�ɑJ�ڂ��s���Ɩ������[�v�Ɋׂ�\��������̂Œe��
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        if (inSightObject?.SightableType == SightableType.Treasure ||
            inSightObject?.SightableType == SightableType.Enemy)
        {
            ChangeState(StateType.Select);
            return;
        }

        if (_stateMachine.StateControl.IsTargetPosArrival() && !_isArraival)
        {
            _isArraival = true;
            _stateMachine.StateControl.PlayAnimation("LookAround");

            float delayTime = _stateMachine.StateControl.GetAnimationClipLength("LookAround");
            _tween = DOVirtual.DelayedCall(delayTime, () => TryChangeState(StateType.Explore))
                .SetLink(_stateMachine.gameObject);
        }

        // ���C�����ȉ��̎�
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
        _stateMachine.StateControl.MoveCancel();
    }
}
