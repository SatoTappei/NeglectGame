using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    State _currentState = State.NoTarget;

    public void InitOnStart()
    {
        // TODO:他の個所でも取得処理があったら依存関係の解消を別の所に移す
        _pathfinding = GameObject.FindGameObjectWithTag(PathfindingTag).GetComponent<IPathfinding>();
        _waypointManage = GameObject.FindGameObjectWithTag(WaypointTag).GetComponent<IWaypointManage>();

        _actorPathfindingWaypoint = new(_waypointManage.WaypointDic, transform.position);
        _actorPathfindingMove = new (gameObject, _model, _moveSpeed, _runSpeedMag);
    }

    public bool IsArrivalTargetPos() => _currentState == State.Arraival;

    public void MoveToNextWaypoint()
    {
        Vector3 targetPos = _actorPathfindingWaypoint.Get(WaypointType.Pass);
        Move(_actorPathfindingMove.MoveFollowPath, targetPos);
    }

    public void MoveToExit()
    {
        Vector3 targetPos = _actorPathfindingWaypoint.ExitPos;
        Move(_actorPathfindingMove.MoveFollowPath, targetPos);
    }

    public void MoveTo(Vector3 targetPos)
    {
        Move(_actorPathfindingMove.RunFollowPath, targetPos);
    }

    void Move(UnityAction<Stack<Vector3>, UnityAction> moveMethod, Vector3 targetPos)
    {
        _actorPathfindingMove.MoveCancel();
        _currentState = State.Moving;

        Stack<Vector3> path = _pathfinding.GetPathToTargetPos(transform.position, targetPos);
        moveMethod(path, () => _currentState = State.Arraival);
    }

    public void MoveCancel() => _actorPathfindingMove.MoveCancel();
}