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

    ActorInSightFilter _actorInSightFilter;

    void Awake()
    {
        _actorInSightFilter = new();
    }

    void Start()
    {
        _actorSight.StartLookInSight();
    }

    void Update()
    {
        //_actorSight.Execute();
        //_actorStateMachine.Execute();
    }

    /* 
     *  最優先:プレイヤーのステートマシン作り直し 
     */

    // Sightで全てのSightableObjectを発見
    // 毎フレームSightableObjectがnullじゃないか監視
    // null以外だったらselectに遷移
    // SightableObjectごとに処理を分ける
    // 移動/Sequence

    // 通路のWaypointはランダムな何回でも同じ個所に移動する。ただし、同じ個所に2連続では移動しない
    // 一度発見したTreasure/Enemy/RoomEntranceは二度と発見しない。
    // 
    // 視界の機能がOnになっているのはExploreとMoveToEntrance状態、それ以外ではoff

    // Treasure/Enemy/RoomEntranceはSightから取得される
    // MoveSystem側で管理しているもの Pass/RoomEntrance/Exit
    // ここにSight側の物を管理させるのは良くない。
    // 仲介役が欲しい
    // Sightで発見 => 仲介役が移動可能か判定 => Actorのインタフェース経由ですてとましんに渡す
    // ↑ｲﾏｺｺ

    // Sightで発見したオブジェクトは移動と同時にnullにするべき、そうすることでステートの切り替えで悩まない
    // 何かを発見した場合は視界機能をoffにしてしまう
    // 移動を開始したと同時に再び視界機能をonにするべき？

    // ほしい:MoveToNoSightable()…移動中も視界の機能を使わない = Sequence用の移動メソッド
    // 

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

    void IStateControl.MoveToWaypoint()
    {
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        _actorMoveSystem.MoveToNextWaypoint();
        _actorAnimation.PlayAnim("Move");
    }

    void IStateControl.MoveToExit()
    {
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        _actorMoveSystem.MoveToExit();
        _actorAnimation.PlayAnim("Move");
    }

    void IStateControl.MoveTo(SightableObject target)
    {
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        _actorMoveSystem.MoveTo(target.transform.position);
        _actorAnimation.PlayAnim("Run");
    }

    void IStateControl.MoveCancel() => _actorMoveSystem.MoveCancel();

    float IStateControl.GetAnimationClipLength(string name) => _actorAnimation.GetStateLength(name);

    bool IStateControl.IsTargetPosArrival() => _actorMoveSystem.IsArrivalTargetPos();

    SightableObject IStateControl.GetInSightAvailableMovingTarget()
    {
        SightableObject inSightObject = _actorSight.CurrentInSightObject;
        if (inSightObject != null)
        {
            // 移動先として使えるオブジェクトが渡された場合、移動し始めるまで視界の機能を止めておく
            SightableObject target = _actorInSightFilter.FilteringAvailableMoving(inSightObject);
            if (target != null)
            {
                _actorSight.StopLookInSight();
                return target;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    //void IStateControl.ToggleSight(bool isActive)
    //{
    //    if (isActive)
    //    {
    //        _actorSight.StartLookInSight();
    //    }
    //    else
    //    {
    //        _actorSight.StopLookInSight();
    //    }
    //}
}