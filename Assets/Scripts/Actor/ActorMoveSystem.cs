using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターを移動させる機能を制御するコンポーネント
/// </summary>
public class ActorMoveSystem : MonoBehaviour
{
    enum State
    {
        NoTarget,
        Moving,
        Arraival,
    }

    static readonly string PathfindingTag = "PathfindingSystem";
    static readonly string WaypointTag = "WaypointSystem";

    // 以下3つのSerializeFieldはこのクラスでは使わず
    // ActorPathfindingMoveクラスのコンストラクタに渡すのに使う
    [Header("進行方向を向かせるModelオブジェクト")]
    [SerializeField] Transform _model;
    [Header("移動速度")]
    [SerializeField] float _moveSpeed = 2.0f;
    [Header("走る際の速度の倍率")]
    [SerializeField] float _runSpeedMag = 2.0f;

    IPathfinding _pathfinding;
    IWaypointManage _waypointManage;
    ActorPathfindingWaypoint _actorPathfindingWaypoint;
    ActorPathfindingMove _actorPathfindingMove;

    State currentState = State.NoTarget;

    void Awake()
    {
        // TODO:他の個所でも取得処理があったら依存関係の解消を別の所に移す
        _pathfinding = GameObject.FindGameObjectWithTag(PathfindingTag).GetComponent<IPathfinding>();
        _waypointManage = GameObject.FindGameObjectWithTag(WaypointTag).GetComponent<IWaypointManage>();

        _actorPathfindingWaypoint = new ActorPathfindingWaypoint(_waypointManage.WaypointDic);
        _actorPathfindingMove = new ActorPathfindingMove(gameObject, _model, _moveSpeed, _runSpeedMag);
    }

    public bool IsArrivalTargetPos() => currentState == State.Arraival;

    public void MoveToNextWaypoint()
    {
        // 次のWaypointに向けて移動する
        // TODO:移動の際にアニメーションさせる方法
        currentState = State.Moving;

        Vector3 targetPos = _actorPathfindingWaypoint.GetPassWaypoint();
        Stack<Vector3> path = _pathfinding.GetPathToWaypoint(transform.position, targetPos);
        _actorPathfindingMove.MoveFollowPath(path, () => 
        {
            currentState = State.Arraival;
        });
    }

    public void MoveToExit()
    {
        // 出口に向けて移動する
        // TODO:移動の際にアニメーションさせる方法
    }

    public void MoveTo(Vector3 targetPos)
    {
        // 指定した位置に移動する
        // TODO:移動の際にアニメーションさせる方法
    }
}