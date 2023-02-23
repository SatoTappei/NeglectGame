using DG.Tweening;

/// <summary>
/// ランダムなWaypointに向けて移動するステートのクラス
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
            ChangeState(StateType.Select);
            return;
        }

        // 目的地に到着したら見回すアニメーションの長さ分だけ待つことで
        // アニメーションの終了を待機しての処理を実現している
        if (_stateMachine.StateControl.IsTargetPosArrival() && !_isArraival)
        {
            _isArraival = true;
            _stateMachine.StateControl.PlayAnimation("LookAround");

            float delayTime = _stateMachine.StateControl.GetAnimationClipLength("LookAround");
            _tween = DOVirtual.DelayedCall(delayTime, () => TryChangeState(StateType.Explore))
                .SetLink(_stateMachine.gameObject);
        }

        // やる気が一定以下の時
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
        _stateMachine.StateControl.MoveCancel();
    }

    /// <summary>
    /// 処理順の関係でInSightSelectステートに遷移する処理が呼び出された後にDelayedCall()が
    /// 呼ばれることもあるため、先に遷移処理が呼ばれていた場合はこの遷移処理をキャンセルする
    /// </summary>
    void TryChangeState(StateType type)
    {
        if (_stage == Stage.Stay)
        {
            ChangeState(type);
        }
        else
        {
            Debug.LogWarning("既に別のステートに遷移する処理が呼ばれています: " + type);
        }
    }
}
