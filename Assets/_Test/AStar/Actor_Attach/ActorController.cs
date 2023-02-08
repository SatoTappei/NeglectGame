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

    ActorStatus _actorStatus;
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    /// <summary>
    /// 各ステートに遷移した時にfalseになり
    /// そのステートの行動が終わったらtrueになってステートからの遷移可能になる
    /// </summary>
    bool _isTransitionable;
    // 仮の見つけたお宝(敵含む)、別のスクリプトに映す
    GameObject _findedTreasure;

    // 次のステートを決める(次のステートに移る際にStateIDをNonにするように直すべき)
    StateID _nextState;

    // ダンジョン脱出目的である敵を倒した時にtrueになる
    bool isTargetDestroyed;

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _nextState = StateID.Non;
        _actorStatus = new ActorStatus();
        _actorHpControl.Init(_actorStatus);
        _actorSight.Init(_actorStatus);

        // MessageBrokerで敵を倒したメッセージを受け取るテスト用
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => isTargetDestroyed = true);
    }

    void IStateControl.PlayAnim(StateID current, StateID next)
    {
        string stateName = StateIDToString(current);

        // stateNameが空だと再生するアニメーションがない
        if (stateName == string.Empty) return;

        _isTransitionable = false;
        _actorAnimation.PlayAnim(stateName, () => 
        {
            _isTransitionable = true;
            _nextState = next;
        });
    }

    string StateIDToString(StateID id)
    {
        switch (id)
        {
            case StateID.Appear:     return "Appear";
            case StateID.Attack:     return "Attack";
            case StateID.Joy:        return "Joy";
            case StateID.LookAround: return "LookAround";
            case StateID.Panic:      return "Panic";
        }

        Debug.LogError("ステートIDが登録されていません:" + id);
        return string.Empty;
    }

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    // 現状移動にもこのフラグが使用されているので、PlayAnimメソッドをよんでいないMoveステートも
    // 移動完了したタイミングでこのメソッドもtrueを返すようになる
    bool IStateControl.IsTransitionable() => _isTransitionable;

    void IStateControl.CancelAnim(string name)
    {
        // アニメーションキャンセル処理
    }

    bool IStateControl.IsDead() => _actorHpControl.IsHpIsZero();

    bool IStateControl.IsTargetLost()
    {
        // 攻撃している対象が倒れたらtrue
        if (isTargetDestroyed)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    void IStateControl.ExploreRandom()
    {
        Stack<Vector3> pathStack = GetPathStack(_pathfindingTarget.GetPathfindingTarget());
        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () => 
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.RunToTarget()
    {
        SightableType target = _findedTreasure.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        Stack<Vector3> pathStack = GetPathStack(_findedTreasure.transform.position);
        _isTransitionable = false;
        _actorAction.RunFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.MoveToExit()
    {
        Stack<Vector3> pathStack = GetPathStack(_pathfindingTarget.GetExitPos());
        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    Stack<Vector3> GetPathStack(Vector3 targetPos)
    {
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    void IStateControl.CancelMoveToTarget()
    {
        _actorAction.MoveCancel();
    }

    bool IStateControl.IsSightTarget()
    {
        GameObject InsightObject = _actorSight.GetInSightObject();
        if (InsightObject != null)
        {
            _findedTreasure = InsightObject;
            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }
}