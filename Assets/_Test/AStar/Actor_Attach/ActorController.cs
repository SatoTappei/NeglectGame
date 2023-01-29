using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] ActorAction _actorAction;
    [Header("System�I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;
    [SerializeField] GameObject _testDestroyedPrefab;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    bool _isTransitionable;

    void Start()
    {
        // TODO:���̎Q�Ɛ�̎擾���@���Ȃ񂩋C�ɂȂ�
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();
    }

    public void MoveToTarget()
    {
        _isTransitionable = false;
        _actorAction.MoveFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void RunToTarget()
    {
        _isTransitionable = false;
        _actorAction.RunFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void CancelMoveToTarget() => _actorAction.MoveCancel();

    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    public bool IsTransitionable() => _isTransitionable;

    public void PlayWanderAnim()
    {
        _isTransitionable = false;
        _actorAction.LookAround(() => _isTransitionable = true);
    }

    // ���v���t�@�N�^�����O
    public void PlayAppearAnim()
    {
        _isTransitionable = false;
        _actorAction.PlayAppearAnim(() => _isTransitionable = true);
    }

    public bool IsTransitionToPanicState()
    {
        // ������������
        return Input.GetKeyDown(KeyCode.W);
    }

    // ���v���t�@�N�^�����O
    public void PlayPanicAnim()
    {
        _isTransitionable = false;
        _actorAction.PlayPanicAnim(() => _isTransitionable = true);
    }

    public bool IsTransitionToDeadState()
    {
        // �����S�𔻒肷��
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Instantiate(_testDestroyedPrefab, transform.position, Quaternion.identity);
    }
}
