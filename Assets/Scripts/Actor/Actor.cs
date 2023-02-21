using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの各コンポーネントを制御するコンポーネント
/// </summary>
public class Actor : MonoBehaviour, IStateControl
{
    [SerializeField] ActorMoveSystem _actorMoveSystem;
    [SerializeField] ActorStateMachine _actorStateMachine;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    void Start()
    {
        _actorSight.StartLookInSight();
    }

    /* 今日のタスク:Statemachineの作成 */

    // 登場演出
    // うろうろ
    //  一定以下のやる気で出口へ移動

    // お宝を見つけた場合
    // ★各ステートでの値の受け渡しはいらない
    //  見つけたアニメーション(アニメーション終了)
    //  対象に向かってダッシュ(位置が来た)
    //  喜ぶ(アニメーション終了)

    // 敵を見つけた場合
    //  見つけたアニメーション(アニメーション終了)
    //  対象に向かってダッシュ(位置に到着)
    //  戦闘する(実際に戦っているわけではない、一定の確率で勝ち負けが決まる)
    //  やる気が閾値以上かどうか判定
    //  出口へ移動

    // ツリー側への参照
    // やる気がどれくらいか
    // 発見するための視界

    // どの状態からも死ねる

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

    void IStateControl.MoveToWaypoint() => _actorMoveSystem.MoveToNextWaypoint();

    void IStateControl.MoveTo(Vector3 targetPos) => _actorMoveSystem.MoveTo(targetPos);

    float IStateControl.GetAnimationClipLength(string name) => _actorAnimation.GetStateLength(name);

    bool IStateControl.IsArrivalTargetPos() => _actorMoveSystem.IsArrivalTargetPos();

    SightableObject IStateControl.GetInSightObject() => _actorSight.CurrentInSightObject;

    void IStateControl.ToggleSight(bool isActive)
    {
        if (isActive)
        {
            _actorSight.StartLookInSight();
        }
        else
        {
            _actorSight.StopLookInSight();
        }
    }
}
