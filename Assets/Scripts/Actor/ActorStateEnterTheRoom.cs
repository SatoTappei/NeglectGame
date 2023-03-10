using DG.Tweening;

/// <summary>
/// 部屋の入口まで移動するステートのクラス
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
            Debug.LogWarning("RoomEntrance種類のオブジェクトを取得できませんでした。");
            return;
        }

        _stateMachine.StateControl.RunTo(inSightObject);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsHpEqualZero())
        {
            TryChangeState(StateType.Dead);
            return;
        }

        // 部屋の入口を見つけた場合に遷移を行うと無限ループに陥る可能性があるので弾く
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
    }

    protected override void Exit()
    {
        ExitProcess();
    }

    public override void OnStateMachinePause()
    {
        ExitProcess();
    }

    void ExitProcess()
    {
        _isArraival = false;
        _tween?.Kill();
        _stateMachine.StateControl.MoveCancel();
    }
}


// 対処:リザルトでも罠を踏んだ音が鳴ってしまう不具合
