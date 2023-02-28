using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// キャラクターの各コンポーネントを制御するコンポーネント
/// </summary>
public class Actor : MonoBehaviour, IStateControl
{
    [SerializeField] ActorMoveSystem _actorMoveSystem;
    [SerializeField] ActorStateMachine _actorStateMachine;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;
    [SerializeField] ActorDisappearPerformance _actorDisappearPerformance;
    [SerializeField] ActorHpModel _actorHpModel;

    ActorInSightFilter _actorInSightFilter;

    void Awake()
    {
        _actorInSightFilter = new();
    }

    /* 
     *  Awake()とOnEnable()の後、Start()の直前に外部で位置を初期化している 
     */

    /* 
     *  次のタスク 
     *  ダンジョン内の全ての部屋を見回っても何もない場合は帰るようにする
     *  敵とお宝のリポップ
     */

    void Start()
    {
        _actorAnimation.Init();
        _actorMoveSystem.Init();
        _actorStateMachine.Init();
        _actorHpModel.Init();

        // Updateとは別のタイミング、周期で呼ばれる
        _actorSight.StartLookInSight();
        _actorHpModel.StartDecreaseHpPerSecond();
    }

    void Update()
    {
        _actorStateMachine.Execute();
    }

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

    void IStateControl.PlayGoalPerformance()
    {
        _actorDisappearPerformance.PlayGoalPerformance();
        _actorMoveSystem.MoveCancel();
        _actorSight.StopLookInSight();
        _actorHpModel.StopDecreaseHpPerSecond();
    }

    void IStateControl.PlayDeadPerformance()
    {
        _actorDisappearPerformance.PlayDeadPerformance();
        _actorMoveSystem.MoveCancel();
        _actorSight.StopLookInSight();
        _actorHpModel.StopDecreaseHpPerSecond();
    }

    void IStateControl.MoveToWaypoint()
    {
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        _actorMoveSystem.MoveToNextWaypoint();
        _actorAnimation.PlayAnim("Move");
    }

    void IStateControl.MoveToExit()
    {
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        _actorMoveSystem.MoveToExit();
        _actorAnimation.PlayAnim("Move");
    }

    void IStateControl.MoveTo(SightableObject target)
    {
        _actorSight.StartLookInSight();
        MoveTo(target);
    }

    void IStateControl.MoveToInactiveLookInSight(SightableObject target)
    {
        MoveTo(target);
    }

    void MoveTo(SightableObject target)
    {
        _actorSight.ResetLookInSight();
        _actorMoveSystem.MoveTo(target.transform.position);
        _actorInSightFilter.AddUnAvailableMovingTarget(target);
        _actorAnimation.PlayAnim("Run");
    }

    void IStateControl.MoveCancel() => _actorMoveSystem.MoveCancel();

    void IStateControl.AffectAroundEffectableObject(string message) => _actorEffecter.EffectAround(message);

    float IStateControl.GetAnimationClipLength(string name) => _actorAnimation.GetStateLength(name);

    bool IStateControl.IsTargetPosArrival() => _actorMoveSystem.IsArrivalTargetPos();

    bool IStateControl.IsBelowHpThreshold() => _actorHpModel.IsBelowHpThreshold();

    SightableObject IStateControl.GetInSightAvailableMovingTarget()
    {
        Queue<SightableObject> inSightObjectQueue = _actorSight.InSightObjectQueue;

        if (inSightObjectQueue.Count > 0)
        {
            SightableObject target = _actorInSightFilter.SelectMovingTarget(inSightObjectQueue);
            if (target != null)
            {
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
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}