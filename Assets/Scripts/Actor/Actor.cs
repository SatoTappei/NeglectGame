using UnityEngine;

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

    ActorInSightFilter _actorInSightFilter;

    /* 
     *  視界の実装がおかしいのできちんとした視界になるように直す
     *  自身が生成されたゴールに戻るように直す
     */

    void Awake()
    {
        _actorInSightFilter = new();
    }

    void Start()
    {
        _actorSight.StartLookInSight();
    }

    void OnDisable()
    {

    }

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

    void IStateControl.PlayGoalPerformance()
    {
        _actorDisappearPerformance.PlayGoalPerformance();
    }

    void IStateControl.PlayDeadPerformance()
    {
        _actorDisappearPerformance.PlayDeadPerformance();
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

    void IStateControl.MoveToNoSight(SightableObject target)
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

    float IStateControl.GetAnimationClipLength(string name) => _actorAnimation.GetStateLength(name);

    bool IStateControl.IsTargetPosArrival() => _actorMoveSystem.IsArrivalTargetPos();

    SightableObject IStateControl.GetInSightAvailableMovingTarget()
    {
        SightableObject inSightObject = _actorSight.CurrentInSightObject;
        if (inSightObject != null)
        {
            // 移動先として使えるオブジェクトが渡された場合、移動し始めるまで視界の機能を止めておく
            SightableObject target = _actorInSightFilter.FilteringAvailableMoving(inSightObject);
            if (target != null)
            {
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
}