using DG.Tweening;

/// <summary>
/// ランダムなWaypointに向けて移動するステートのクラス
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
        if (_stateMachine.StateControl.GetInSightObject() != null)
        {
            ChangeState(StateType.InSightSelect);
            return;
        }

        if (_isArraival) return;

        // 目的地に到着したら見回すアニメーションの長さ分だけ待つことで
        // アニメーションの終了を待機しての処理を実現している
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

        // やる気が一定以下の時
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
