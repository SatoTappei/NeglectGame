using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IStateControlOld
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorPathfindingMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;
    [SerializeField] DamagedMessageReceiver _damagedMessageReceiver;

    ActorControlHelper _actorControlHelper = new ActorControlHelper();
    ITargetSelectable _targetSelectable;
    IPathfinding _pathGetable;
    StateIDOld _nextState = StateIDOld.Non;
    /// <summary>
    /// 各ステートに遷移した時にfalseになり、そのステートの行動が終わったら
    /// trueになってステートからの遷移可能になる
    /// </summary>
    bool _isTransitionable;
    /// <summary>このフラグがtrueになった場合、次は必ずEscapeステートに遷移する</summary>
    bool _isCompleted;

    /*
     * 次やること
     * パーツは一通り動くものは完成したのでランダム生成しない地形で一通りゲームになるか試す。
     * ・InGameStreamを作り、ゲーム全谷の流れを追いながら作るs
     * ・簡易的なUI/演出やらなんやらも作り始める
     * ・オブジェクトのリポップ機能
     * ・InGameStreamを作るのでシーン全体の管理が可能になる
     * ・各種コンポーネントの手直しをする
     */

    // どんなゲーム？
    // ダンジョンに入ってくる冒険者を罠で邪魔するゲーム
    // プレイヤーはダンジョンに罠を置ける
    // 冒険者が罠を踏むとダメージ
    // たくさんの冒険者を葬ろう

    void Awake()
    {
        _damagedMessageReceiver.OnDefeated += () => _isCompleted = true;
    }

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _targetSelectable = system.GetComponent<ITargetSelectable>();
        _pathGetable = system.GetComponent<IPathfinding>();
    }

    void IStateControlOld.PlayAnim(StateIDOld current, StateIDOld next)
    {
        string stateName = _actorControlHelper.StateIDToString(current);

        // アニメーションの再生が完了したタイミングでisTransitionableがtrueになることで
        // _nextStateへの遷移が可能になる
        _isTransitionable = false;
        _actorAnimation.PlayAnim(stateName, () => 
        {
            _isTransitionable = true;
            _nextState = next;
        });
    }

    void IStateControlOld.MoveToRandomWaypoint()
    {
        _nextState = StateIDOld.LookAround;
        MoveTo(_targetSelectable.GetNextWaypointPos(), _actorAction.MoveFollowPath);
    }

    void IStateControlOld.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject.gameObject;
        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateIDOld.Attack : StateIDOld.Joy;

        MoveTo(inSightObject.transform.position, _actorAction.RunFollowPath);
    }

    void IStateControlOld.MoveToExit()
    {
        _nextState = StateIDOld.LookAround;
        MoveTo(_targetSelectable.GetExitPos(), _actorAction.MoveFollowPath);
    }

    void MoveTo(Vector3 targetPos, UnityAction<Stack<Vector3>, UnityAction> moveMethod)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathToWaypoint(transform.position, targetPos);

        // ターゲットへの移動が完了したタイミングでisTransitionableがtrueになることで
        // _nextStateへの遷移が可能になる
        _isTransitionable = false;
        moveMethod(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControlOld.CancelMoving() => _actorAction.MoveCancel();

    bool IStateControlOld.IsEqualNextState(StateIDOld state) => _nextState == state;

    bool IStateControlOld.IsTransitionable() => _isTransitionable;

    bool IStateControlOld.IsDead() => _actorHpControl.IsHpEqualZero();

    bool IStateControlOld.IsCompleted()
    {
        if (_isCompleted)
        {
            _nextState = StateIDOld.Escape;
            return true;
        }

        return false;
    }

    bool IStateControlOld.IsSightTarget()
    {
        if (_actorSight.IsFindInSight())
        {
            SightableObject inSightObject = _actorSight.CurrentInSightObject;
            
            if (inSightObject.HasWitness()) return false;
            inSightObject.SetWitness(gameObject);

            _nextState = StateIDOld.Panic;
            return true;
        }

        return false;
    }

    void IStateControlOld.EffectAroundEffectableObject() => _actorEffecter.EffectAround();
}