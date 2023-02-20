using DG.Tweening;

/// <summary>
/// ActorStateMachineで使用する見つけた対象に向けて移動するステートのクラス
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
        Debug.Log("何か見つけた");
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
        //    //  宝箱を見つけた
        //    // Sequenceの実行
        //}
        //else if (inSightObject?.SightableType == SightableType.Enemy)
        //{
        //    //  敵を発見した
        //    //      n%の確率で結果が勝ち/負けのステートに遷移
        //    // Sequenceの実行
        //}

        //if (inSightObject?.SightableType == SightableType.Waypoint)
        //{
        //    //  部屋を発見した
        //    // 部屋の中へ入る
        //    // うろうろへ戻る
        //    // ステートが切り替わるのは次のフレームから
        //    // もし呼び出し順の関係でこのフレーム内でInSightObjectが変わってしまったら
        //    // 見つけたWaypointに向かう処理のはずが宝や敵に向かって歩いて行ってしまう。

        //    // 対処:何かを発見したら発見したものに向けて歩いていくステートに遷移する
        //    //      そのステート内で発見処理を行い
        //    //      宝/敵を発見したら各Sequenceに遷移する

        //    return;
        //}
        //else if(inSightObject?.SightableType == SightableType.Treasure)
        //{
        //    //  宝箱を見つけた
        //}
        //else if (inSightObject?.SightableType == SightableType.Enemy)
        //{
        //    //  敵を発見した
        //    //      n%の確率で結果が勝ち/負けのステートに遷移
        //}
    }

    protected override void Exit()
    {
        _isArraival = false;
        _tween?.Kill();
    }
}
