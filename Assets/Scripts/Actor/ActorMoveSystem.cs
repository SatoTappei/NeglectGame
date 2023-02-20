using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    State currentState = State.NoTarget;

    void Awake()
    {
        // TODO:���̌��ł��擾��������������ˑ��֌W�̉�����ʂ̏��Ɉڂ�
        _pathfinding = GameObject.FindGameObjectWithTag(PathfindingTag).GetComponent<IPathfinding>();
        _waypointManage = GameObject.FindGameObjectWithTag(WaypointTag).GetComponent<IWaypointManage>();

        _actorPathfindingWaypoint = new (_waypointManage.WaypointDic);
        _actorPathfindingMove = new (gameObject, _model, _moveSpeed, _runSpeedMag);
    }

    public bool IsArrivalTargetPos() => currentState == State.Arraival;

    public bool IsOnMovableMass()
    {
        // ���g����������
        return true;
    }

    public void MoveToNextWaypoint()
    {
        _actorPathfindingMove.MoveCancel();

        // ����Waypoint�Ɍ����Ĉړ�����
        // TODO:�ړ��̍ۂɃA�j���[�V������������@
        currentState = State.Moving;

        Vector3 targetPos = _actorPathfindingWaypoint.GetPassWaypoint();
        Stack<Vector3> path = _pathfinding.GetPathToWaypoint(transform.position, targetPos);

        // Grid����o�Ă��܂����^�C�~���O�Ōo�H�T�����Ă΂ꂽ�ꍇ�Ƀp�X��null�ɂȂ��Ă��܂��̂�
        // ���g�����ǂ��Ă������W���g���Čo�H�T�������邱�Ƃŋߎ��𓾂�
        if (path == null)
        {
            Debug.LogWarning("Grid�O�̍��W����o�H�T�����J�n����܂���" + transform.position);

            //foreach (Vector3 startPos in _prevWalkablePosList)
            //{
            //    path = _pathfinding.GetPathToWaypoint(startPos, targetPos);

            //    if (path != null) break;
            //}
        }

        _actorPathfindingMove.MoveFollowPath(path, () => 
        {
            currentState = State.Arraival;
        });
    }

    public void MoveToExit()
    {
        // �o���Ɍ����Ĉړ�����
        // TODO:�ړ��̍ۂɃA�j���[�V������������@
    }

    public void MoveTo(Vector3 targetPos)
    {
        _actorPathfindingMove.MoveCancel();

        // �w�肵���ʒu�Ɉړ�����
        // TODO:�ړ��̍ۂɃA�j���[�V������������@

        // �G���[:�}�X����}�X�ֈړ�����Ƃ��Ɏ΂߈ړ�������̂�
        // �ǂ̊p���΂߈ړ�����Ƃ��Ɉꎞ�I��grid�͈̔͊O�ɏo�Ă��܂�
        // ���̃^�C�~���O�ł��̃��\�b�h���Ă΂��ƈړ��ł��Ȃ��ӏ�����̈ړ���null���Ԃ�

        currentState = State.Moving;

        Stack<Vector3> path = _pathfinding.GetPathToWaypoint(transform.position, targetPos);

        // Grid����o�Ă��܂����^�C�~���O�Ōo�H�T�����Ă΂ꂽ�ꍇ�Ƀp�X��null�ɂȂ��Ă��܂��̂�
        // ���g�����ǂ��Ă������W���g���Čo�H�T�������邱�Ƃŋߎ��𓾂�
        if (path == null)
        {
            Debug.LogWarning("Grid�O�̍��W����o�H�T�����J�n����܂���" + transform.position);

            //foreach(Vector3 startPos in _prevWalkablePosList)
            //{
            //    path = _pathfinding.GetPathToWaypoint(startPos, targetPos);

            //    if (path != null) break;
            //}
        }

        _actorPathfindingMove.MoveFollowPath(path, () => 
        {
            currentState = State.Arraival;
        });
    }
}