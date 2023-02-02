using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorAction _actorAction;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;

    ActorStatus _actorStatus;
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    /// <summary>
    /// 各ステートに遷移した時にfalseになり
    /// そのステートの行動が終わったらtrueになってステートからの遷移可能になる
    /// </summary>
    bool _isTransitionable;
    // お宝を見つけたらそれを手に入れるまで次の目標に向かわない
    bool _isTreasureable;

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _actorStatus = new ActorStatus();
        _actorHpControl.Init(_actorStatus);
        _actorSight.Init(_actorStatus);
    }

    public bool IsTransitionable() => _isTransitionable;

    public void MoveToTarget() => WaitUntilArrival(_actorAction.MoveFollowPath);
    //public void RunToTarget() => WaitUntilArrival(_actorAction.RunFollowPath);
    public void RunToTarget()
    {
        GameObject target = _actorStatus.Treasure;
        Stack<Vector3> stack = _pathGetable.GetPathStack(transform.position, target.transform.position);
        _isTransitionable = false;
        _actorAction.RunFollowPath(stack, () => _isTransitionable = true);
    }
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

    public void PlayJoyAnim() => WaitAnimFinish(_actorAction.PlayJoyAnim);

    public void PlayAttackAnim() => WaitAnimFinish(_actorAction.PlayAttackAnim);

    public void PlayLookAroundAnim() => WaitAnimFinish(_actorAction.PlayLookAroundAnim);
    public void PlayAppearAnim() => WaitAnimFinish(_actorAction.PlayAppearAnim);
    public void PlayPanicAnim() => WaitAnimFinish(_actorAction.PlayPanicAnim);

    void WaitAnimFinish(UnityAction<UnityAction> unityAction)
    {
        _isTransitionable = false;
        unityAction(() => _isTransitionable = true);
    }

    /* 
     *  TODO:一度お宝を見つけた後にもう一度お宝を見つけるとループしてしまう不具合
     *       一度お宝を見つけると対象のお宝を手に入れるまで発見しないようにする
     *       フラグのオンオフとか？
     */

    /*  発見したかどうかのboolが返る
     *  PanicステートとRunステートが完全に分離している
     *  Panicステートの次のRunステートでは見つけたお宝に向けて走っていくようにしたい
     *  お宝の座標を渡せばその座標まで経路探索して歩いてくれる
     *  1.お宝発見
     *  2.Panicステートに遷移
     *  3.ランダムな箇所を選択 <= ここを変えたい
     *  4.Runステートでその座標に向かって走る
     *  
     */
    public bool IsTransitionToPanicState()
    {
        if (_isTreasureable) return false;

        bool b = _actorSight.IsFindTreasure();
        if (b)
        {
            _isTreasureable = true;
        }

        return b;
    }
    public bool IsTransitionToDeadState() => _actorHpControl.IsHpIsZero();

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Debug.Log("死んだときになんか演出をする");
    }

    public void RunEndable()
    {
        _isTreasureable = false;
    }
}