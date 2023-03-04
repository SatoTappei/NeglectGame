using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// キャラクターの各コンポーネントを制御するコンポーネント
/// </summary>
public class Actor : MonoBehaviour, IStateControl
{
    [SerializeField] ActorStatusHolder _actorStatusHolder;
    [SerializeField] ActorMoveSystem _actorMoveSystem;
    [SerializeField] ActorStateMachine _actorStateMachine;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;
    [SerializeField] ActorPerformance _actorPerformance;
    [SerializeField] ActorHpControl _actorHpModel;

    ActorInSightFilter _actorInSightFilter;

    void Awake()
    {
        _actorInSightFilter = new();
        _actorStateMachine.InitOnAwake();
    }

    /* 
     *  Awake()とOnEnable()の後、Start()の直前に外部で位置を初期化している 
     */

    /* 
     *  次のタスク 
     *  ダンジョン内の全ての部屋を見回っても何もない場合は帰るようにする
     *  台詞表示機能
     *  死体にコライダーが残っているので罠が反応してしまう、ので直す
     *  音
     *  ゲームとしての体裁(タイトル=>インゲーム=>終了=>リロード)を整える
     *  UIの画像とか
     *  ゲームスタートの演出の作成(エラーの原因になるので必須)
     */

    void Start()
    {
        _actorAnimation.InitOnStart();
        _actorMoveSystem.InitOnStart();
        _actorHpModel.InitOnStart(_actorStatusHolder.MaxHp);

        // Updateとは別のタイミング、周期で呼ばれる
        _actorSight.StartLookInSight();
        _actorHpModel.StartDecreaseHpPerSecond();
    }

    void Update()
    {
        _actorStateMachine.Execute();
    }

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

    void IStateControl.PlayGoalPerformance() => PlayPerformance(_actorPerformance.PlayGoalPerformance);
    void IStateControl.PlayDeadPerformance() => PlayPerformance(_actorPerformance.PlayDeadPerformance);
    void IStateControl.MoveToWaypoint() => MoveTo(_actorMoveSystem.MoveToNextWaypoint);
    void IStateControl.MoveToExit() => MoveTo(_actorMoveSystem.MoveToExit);
    void IStateControl.RunToInactiveLookInSight(SightableObject target) => RunTo(target);
    void IStateControl.RunTo(SightableObject target)
    {
        _actorSight.StartLookInSight();
        RunTo(target);
    }
    void IStateControl.MoveCancel() => _actorMoveSystem.MoveCancel();

    void PlayPerformance(UnityAction performance)
    {
        performance();
        _actorMoveSystem.MoveCancel();
        _actorSight.StopLookInSight();
        _actorHpModel.DecreaseHp(999);
        _actorHpModel.StopDecreaseHpPerSecond();
    }

    void MoveTo(UnityAction moveTo)
    {
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        moveTo();
        _actorAnimation.PlayAnim("Move");
    }

    void RunTo(SightableObject target)
    {
        _actorSight.ResetLookInSight();
        _actorMoveSystem.MoveTo(target.transform.position);
        _actorInSightFilter.AddUnAvailableMovingTarget(target);
        _actorAnimation.PlayAnim("Run");
    }

    void IStateControl.AffectAroundEffectableObject(string message) => _actorEffecter.EffectAround(message);
    float IStateControl.GetAnimationClipLength(string name) => _actorAnimation.GetStateLength(name);
    bool IStateControl.IsTargetPosArrival() => _actorMoveSystem.IsArrivalTargetPos();
    bool IStateControl.IsBelowHpThreshold() => _actorHpModel.IsBelowHpThreshold();
    bool IStateControl.IsHpEqualZero() => _actorHpModel.IsHpEqualZero();
    SightableObject IStateControl.GetInSightAvailableMovingTarget()
    {
        Queue<SightableObject> inSightObjectQueue = _actorSight.InSightObjectQueue;
        if (inSightObjectQueue.Count == 0) return null;
        
        SightableObject target = _actorInSightFilter.SelectMovingTarget(inSightObjectQueue);
        if (target == null) return null;

        if (target.IsAvailable(this))
        {
            // 移動先として使えるオブジェクトが渡された場合、移動し始めるまで視界の機能を止めておく
            _actorSight.StopLookInSight();
            return target;
        }
        else
        {
            return null;
        }
    }


}