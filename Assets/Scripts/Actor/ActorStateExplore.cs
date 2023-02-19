using DG.Tweening;

/// <summary>
/// ActorStateMachineで使用するランダムなWaypointに向けて移動するステートのクラス
/// </summary>
public class ActorStateExplore : ActorStateBase
{
    bool _isArraival;

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
            Debug.Log("うろうろおわり");
            _stateMachine.StateControl.PlayAnimation("Panic");

            float delayTime = _stateMachine.StateControl.GetAnimationClipLength("Panic");
            DOVirtual.DelayedCall(delayTime, () =>
            {
                ChangeState(StateType.Explore);
            }).SetLink(_stateMachine.gameObject);
        }

        // やる気が一定以下の時

        // 宝箱を見つけたとき

        // 敵を発見したとき
        //  n%の確率で結果が勝ち/負けのステートに遷移
    }

    protected override void Exit()
    {
        _isArraival = false;
    }
}
