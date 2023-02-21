using DG.Tweening;

/// <summary>
/// 部屋の入口まで移動するステートのクラス
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
        // このステートに遷移する前に視界の機能を切ってあるので、前フレームから視界が更新されず
        // RoomEntrance種類のオブジェクトが取得できる
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        _stateMachine.StateControl.MoveTo(inSightObject.gameObject.transform.position);

        // 視界にとらえたものに応じてSequenceを実行したいので再度ここで視界の機能をオンにしている
        _stateMachine.StateControl.ToggleSight(isActive: true);
    }

    protected override void Stay()
    {
        // 部屋の入口を見つけた場合に遷移を行うと無限ループに陥る可能性があるので弾く
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        if (inSightObject?.SightableType == SightableType.Treasure ||
            inSightObject?.SightableType == SightableType.Enemy)
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
