using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[���ړ�������@�\�̃R���|�[�l���g
/// </summary>
public class ActorMoveSystem : MonoBehaviour
{
    static readonly string PathfindingTag = "PathfindingSystem";
    static readonly string WaypointTag = "WaypointSystem";

    // ���̃N���X�ł͎g�킸�AActorPathfindingMove�N���X�̃R���X�g���N�^�ɓn���̂Ɏg��
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

    void Awake()
    {
        // TODO:���̌��ł��擾��������������ˑ��֌W�̉�����ʂ̏��Ɉڂ�
        _pathfinding = GameObject.FindGameObjectWithTag(PathfindingTag).GetComponent<IPathfinding>();
        _waypointManage = GameObject.FindGameObjectWithTag(WaypointTag).GetComponent<IWaypointManage>();

        _actorPathfindingWaypoint = new ActorPathfindingWaypoint(_waypointManage.WaypointDic);
        _actorPathfindingMove = new ActorPathfindingMove(gameObject, _model, _moveSpeed, _runSpeedMag);
    }

    void Start()
    {
        // �e�X�g�p
        MoveToNextWaypoint();
    }

    public void MoveToNextWaypoint()
    {
        // ����Waypoint�Ɍ����Ĉړ�����
        // TODO:�ړ��̍ۂɃA�j���[�V������������@

        Vector3 waypoint = _actorPathfindingWaypoint.GetPassWaypoint();
        Stack<Vector3> path = _pathfinding.GetPathToWaypoint(transform.position, waypoint);
        _actorPathfindingMove.MoveFollowPath(path, null);
    }

    public void MoveToExit()
    {
        // �o���Ɍ����Ĉړ�����
        // TODO:�ړ��̍ۂɃA�j���[�V������������@
    }

    public void MoveTo(Vector3 targetPos)
    {
        // �w�肵���ʒu�Ɉړ�����
        // TODO:�ړ��̍ۂɃA�j���[�V������������@
    }
}