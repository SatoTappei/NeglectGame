using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorAction _actorAction;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    /// <summary>
    /// 各ステートに遷移した時にfalseになり
    /// そのステートの行動が終わったらtrueになってステートからの遷移可能になる
    /// </summary>
    bool _isTransitionable;

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
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
        Debug.Log("死んだときになんか演出をする");
    }
}
