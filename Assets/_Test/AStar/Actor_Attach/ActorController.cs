using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    /* 
     *  オリジナルの規約に乗っ取り、LSystemのほうももう一度リファクタリングする 
     */

    // 攻撃ステートの挙動
    // キャラクターと敵がアタックモーションを繰り返す
    // ターン制ではない
    // 敵の攻撃モーションがヒットするたびにHPが削られていく
    // 敵が倒れたという判定をどうするか
    //  死亡した際にメッセージングしたい
    //  メッセージングしない場合、コライダーに当たったキャラクターの攻撃キャンセル処理を呼ぶ必要がある
    //  別のキャラに手柄を横取りされた場合はMoveステートに戻ってまたうろうろするしかない？
    //  そもそも一人しか発見できないようにするべき
    //  お宝も同様にする必要がある
    //  これらの判定はSightableObjectコンポーネントでどうにかできそう
    //  敵側にダメージを与える処理を書く必要はない(アニメーションのタイミングでキャラのHPを減らせばそう見える)
    //  ↑でも良いかもしれない

    // 現在の状態の欠点:どのステートにどれが使われているかがはっきりしない
    //                  ただそれはインターフェースがどっちに依存しているかの問題

    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    ActorControlHelper _actorControlHelper;
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    // 次のステートを決める(次のステートに移る際にStateIDをNonにするように直すべき)
    StateID _nextState;
    /// <summary>
    /// 各ステートに遷移した時にfalseになり、そのステートの行動が終わったら
    /// trueになってステートからの遷移可能になる
    /// </summary>
    bool _isTransitionable;
    /// <summary>このフラグがtrueになった場合、次は必ずEscapeステートに遷移する</summary>
    bool _isPurposeCompleted;

    void Awake()
    {
        _actorControlHelper = new ActorControlHelper();
    }

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _nextState = StateID.Non;

        // MessageBrokerで敵を倒したメッセージを受け取るテスト用
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => _isPurposeCompleted = true);
    }

    void IStateControl.PlayAnim(StateID current, StateID next)
    {
        string stateName = _actorControlHelper.StateIDToString(current);

        _isTransitionable = false;
        _actorAnimation.PlayAnim(stateName, () => 
        {
            _isTransitionable = true;
            _nextState = next;
        });
    }

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    // 現状移動にもこのフラグが使用されているので、PlayAnimメソッドをよんでいないMoveステートも
    // 移動完了したタイミングでこのメソッドもtrueを返すようになる
    bool IStateControl.IsTransitionable() => _isTransitionable;

    void IStateControl.CancelAnim(string name)
    {
        // アニメーションキャンセル処理
    }

    bool IStateControl.IsDead() => _actorHpControl.IsHpEqualZero();

    bool IStateControl.IsTargetLost()
    {
        // メッセージの受信は専用のメッセージReceiverを作ってそっちで受け取る

        // 攻撃している対象が倒れたらtrue
        if (_isPurposeCompleted)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    void IStateControl.ExploreRandom()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () => 
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject;

        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        Vector3 targetPos = inSightObject.transform.position;
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        _actorAction.RunFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.MoveToExit()
    {
        Vector3 targetPos = _pathfindingTarget.GetExitPos();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.CancelMoveToTarget()
    {
        _actorAction.MoveCancel();
    }

    bool IStateControl.IsSightTarget()
    {
        // TODO:他のキャラが発見状態にならないように対象の発見可能フラグを切り替える必要がある
        if (_actorSight.IsFindInSight())
        {
            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.EffectAround() => _actorEffecter.EffectAround();
}