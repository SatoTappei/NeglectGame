using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[���ړ�������@�\�𐧌䂷��R���|�[�l���g
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

    // �ȉ�3��SerializeField�͂��̃N���X�ł͎g�킸
    // ActorPathfindingMove�N���X�̃R���X�g���N�^�ɓn���̂Ɏg��
    [Header("�i�s��������������Model�I�u�W�F�N�g")]
    [SerializeField] Transform _model;
    [Header("�ړ����x")]
    [SerializeField] float _moveSpeed = 2.0f;
    [Header("����ۂ̑��x�̔{��")]
    [SerializeField] float _runSpeedMag = 2.0f;

    IPathfinding _pathfinding;
    IWaypointManage _waypointManage;
    ActorPathfindingWaypoint _actorPathfindingWaypoint;
    ActorPathfindingMove _actorPathfindingMove;

    State _currentState = State.NoTarget;

    void Awake()
    {
        // TODO:���̌��ł��擾��������������ˑ��֌W�̉�����ʂ̏��Ɉڂ�
        _pathfinding = GameObject.FindGameObjectWithTag(PathfindingTag).GetComponent<IPathfinding>();
        _waypointManage = GameObject.FindGameObjectWithTag(WaypointTag).GetComponent<IWaypointManage>();

        _actorPathfindingWaypoint = new (_waypointManage.WaypointDic);
        _actorPathfindingMove = new (gameObject, _model, _moveSpeed, _runSpeedMag);
    }

    public bool IsArrivalTargetPos() => _currentState == State.Arraival;

    public void MoveToNextWaypoint()
    {
        _actorPathfindingMove.MoveCancel();
        _currentState = State.Moving;

        Vector3 targetPos = _actorPathfindingWaypoint.Get(WaypointType.Pass);
        Stack<Vector3> path = _pathfinding.GetPathToTargetPos(transform.position, targetPos);
        _actorPathfindingMove.MoveFollowPathAsync(path, () =>
        {
            _currentState = State.Arraival;
        }).Forget();
    }

    public void MoveToExit()
    {
        _actorPathfindingMove.MoveCancel();
        _currentState = State.Moving;

        Vector3 targetPos = _actorPathfindingWaypoint.Get(WaypointType.Exit);
        Stack<Vector3> path = _pathfinding.GetPathToTargetPos(transform.position, targetPos);
        _actorPathfindingMove.MoveFollowPathAsync(path, () =>
        {
            _currentState = State.Arraival;
        }).Forget();
    }

    public void MoveTo(Vector3 targetPos)
    {
        _actorPathfindingMove.MoveCancel();
        _currentState = State.Moving;

        Stack<Vector3> path = _pathfinding.GetPathToTargetPos(transform.position, targetPos);
        _actorPathfindingMove.MoveFollowPathAsync(path, () =>
        {
            _currentState = State.Arraival;
        }).Forget();
    }

    public void MoveCancel() => _actorPathfindingMove.MoveCancel();
}