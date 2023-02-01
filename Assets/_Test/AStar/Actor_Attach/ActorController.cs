using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    // 渡された値でキャラクターを動かすことは出来たのでキャラクターの状態を作っていく
    // 意欲が無くなると云々だとか、目的を達成したら云々だとか作る

    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorAction _actorAction;
    [SerializeField] ActorStatus _actorStatus;

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

    public bool IsTransitionable() => _isTransitionable;

    public void MoveToTarget() => WaitUntilArrival(_actorAction.MoveFollowPath);
    public void RunToTarget() => WaitUntilArrival(_actorAction.RunFollowPath);
    public void CancelMoveToTarget() => _actorAction.MoveCancel();

    void WaitUntilArrival(UnityAction<Stack<Vector3>, UnityAction> unityAction)
    {
        _isTransitionable = false;
        unityAction(GetPathStack(), () => _isTransitionable = true);
    }

    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    public void PlayWanderAnim() => WaitAnimFinish(_actorAction.PlayLookAroundAnim);
    public void PlayAppearAnim() => WaitAnimFinish(_actorAction.PlayAppearAnim);
    public void PlayPanicAnim() => WaitAnimFinish(_actorAction.PlayPanicAnim);

    void WaitAnimFinish(UnityAction<UnityAction> unityAction)
    {
        _isTransitionable = false;
        unityAction(() => _isTransitionable = true);
    }

    public bool IsTransitionToPanicState()
    {
        // ★何か見つけた
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool IsTransitionToDeadState()
    {
        // ★現在は体力が一定以下で死亡になっているのでメソッドが出来たら直す
        return _actorStatus.IsBelowMotivationThreshold();
    }

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Debug.Log("死んだときになんか演出をする");
    }
}