using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    ActorControlHelper _actorControlHelper = new ActorControlHelper();
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;
    StateID _nextState = StateID.Non;
    /// <summary>
    /// 各ステートに遷移した時にfalseになり、そのステートの行動が終わったら
    /// trueになってステートからの遷移可能になる
    /// </summary>
    bool _isTransitionable;
    /// <summary>このフラグがtrueになった場合、次は必ずEscapeステートに遷移する</summary>
    bool _isCompleted;

    // 優先タスク
    // Moveの処理とisTransitionable変数で一括で管理していいのか考える
    // 敵を倒した時のメッセージング処理をどうにかする
    // 戦闘するのでダメージを受ける処理を作る
    // オブジェクトのリポップ機能
    // 脱出の際の演出
    // ランダム生成しない地形で一通りゲームになるか試す
    
    // どんなゲーム？
    // ダンジョンに入ってくる冒険者を罠で邪魔するゲーム
    // プレイヤーはダンジョンに罠を置ける
    // 冒険者が罠を踏むとダメージ
    // たくさんの冒険者を葬ろう

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        // MessageBrokerで敵を倒したメッセージを受け取るテスト用
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => _isCompleted = true);
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

    void IStateControl.MoveToRandomWaypoint()
    {
        _nextState = StateID.LookAround;
        Move(_pathfindingTarget.GetPathfindingTarget(), _actorAction.MoveFollowPath);
    }

    void IStateControl.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject.gameObject;
        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        Move(inSightObject.transform.position, _actorAction.RunFollowPath);
    }

    void IStateControl.MoveToExit()
    {
        _nextState = StateID.LookAround;
        Move(_pathfindingTarget.GetExitPos(), _actorAction.MoveFollowPath);
    }

    void Move(Vector3 targetPos, UnityAction<Stack<Vector3>, UnityAction> moveMethod)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        moveMethod(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.CancelMoving() => _actorAction.MoveCancel();

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    bool IStateControl.IsTransitionable() => _isTransitionable;

    bool IStateControl.IsDead() => _actorHpControl.IsHpEqualZero();

    bool IStateControl.IsCompleted()
    {
        if (_isCompleted)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    bool IStateControl.IsSightTarget()
    {
        if (_actorSight.IsFindInSight())
        {
            SightableObject inSightObject = _actorSight.CurrentInSightObject;
            
            if (inSightObject.HasWitness()) return false;
            inSightObject.SetWitness(gameObject);

            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.EffectAroundEffectableObject() => _actorEffecter.EffectAround();
}