using DG.Tweening;

/// <summary>
/// ActorStateMachineで使用するランダムなWaypointに向けて移動するステートのクラス
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

        // 目的地に到着したら見回すアニメーションを再生した後に遷移する
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

        // 何かを発見したとき
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        if (inSightObject?.SightableType == SightableType.Waypoint)
        {
            //  部屋を発見した
            // 部屋の中へ入る
            // うろうろへ戻る
        }
        else if(inSightObject?.SightableType == SightableType.Treasure)
        {
            //  宝箱を見つけた
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            //  敵を発見した
            //      n%の確率で結果が勝ち/負けのステートに遷移
        }

        // やる気が一定以下の時
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
