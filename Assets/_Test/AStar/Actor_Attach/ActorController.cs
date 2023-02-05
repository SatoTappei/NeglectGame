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

    void Update()
    {
        // テスト用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _nextState = StateID.Dead;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _nextState = StateID.Panic;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _nextState = StateID.Attack;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            _nextState = StateID.Joy;
        }
    }

    void IStateControl.PlayAnim(string name)
    {
        _isTransitionable = false;

        switch (name)
        {
            case "Appear":
                _actorAction.PlayAppearAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Move;
                });
                break;
            case "LookAround":
                _actorAction.PlayLookAroundAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Move;
                });
                break;
            case "Panic":
                _actorAction.PlayPanicAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Run;
                });
                break;
            case "Attack":
                _actorAction.PlayAttackAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Non;
                });
                break;
            case "Joy":
                _actorAction.PlayJoyAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Non;
                });
                break;
        }
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