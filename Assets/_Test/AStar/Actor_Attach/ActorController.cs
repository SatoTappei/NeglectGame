using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    /* 
     *  オリジナルの規約に乗っ取り、LSystemのほうももう一度リファクタリングする 
     */

    // 各ステートの遷移条件の設定完了、遷移条件に使う各メソッドのリファクタリングをする
    // まずはアニメーションの再生周りのメソッドを整理したい

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

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _nextState = StateID.Non;
        _actorStatus = new ActorStatus();
        _actorHpControl.Init(_actorStatus);
        _actorSight.Init(_actorStatus);
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


    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    void IStateControl.CancelAnim(string name)
    {
        // アニメーションキャンセル処理
    }

    bool IStateControl.IsDead()
    {
        // 死亡したらtrueになる
        return false;
    }

    bool IStateControl.IsTargetLost()
    {
        // ターゲットロスト
        return false;
    }

    void IStateControl.MoveToTarget()
    {
        _isTransitionable = false;
        _actorAction.MoveFollowPath(GetPathStack(), () => 
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.RunToTarget()
    {
        _isTransitionable = false;

        // 仮の分岐処理、見つけたもので分岐する
        // 見つけたものに向かって走るようになっている

        if (_findedTreasure != null)
        {
            if (_findedTreasure.name == "Enemy")
            {
                _nextState = StateID.Attack;
            }
            else
            {
                _nextState = StateID.Joy;
            }
        }

        var pos = _findedTreasure.transform.position;

        _actorAction.RunFollowPath(_pathGetable.GetPathStack(transform.position, pos), () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.CancelMoveToTarget()
    {
        _actorAction.MoveCancel();
    }

    bool IStateControl.IsSightTarget()
    {
        GameObject go = _actorSight.GetInSightObject();
        if (go != null)
        {
            _findedTreasure = go;
        }
        // 毎フレーム呼ばれているので0.3秒おきとかに呼ばれるようにする

        if (_actorSight.IsFindTreasure())
        {
            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.MoveToExit()
    {
        Debug.Log("出口へ");
        // 出口に向かって移動する処理
    }
}