using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] ActorAction _actorAction;
    [Header("Systemオブジェクトのタグ")]
    [SerializeField] string _tag;
    [SerializeField] GameObject _testDestroyedPrefab;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    bool _isTransitionable;

    void Start()
    {
        // TODO:この参照先の取得方法がなんか気になる
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

    // ★要リファクタリング
    public void PlayAppearAnim()
    {
        _isTransitionable = false;
        _actorAction.PlayAppearAnim(() => _isTransitionable = true);
    }

    public bool IsTransitionToPanicState()
    {
        // ★何か見つけた
        return Input.GetKeyDown(KeyCode.W);
    }

    // ★要リファクタリング
    public void PlayPanicAnim()
    {
        _isTransitionable = false;
        _actorAction.PlayPanicAnim(() => _isTransitionable = true);
    }

    public bool IsTransitionToDeadState()
    {
        // ★死亡を判定する
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Instantiate(_testDestroyedPrefab, transform.position, Quaternion.identity);
    }
}
