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

    void Start()
    {
        _actorSight.StartLookInSight();
    }

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

    void IStateControl.MoveToWaypoint()
    {
        _actorMoveSystem.MoveToNextWaypoint();
        _actorAnimation.PlayAnim("Move");
        _actorSight.ResetLookInSight();
    }

    void IStateControl.MoveToExit()
    {
        _actorMoveSystem.MoveToExit();
        _actorAnimation.PlayAnim("Move");
        _actorSight.ResetLookInSight();
    }

    void IStateControl.MoveTo(Vector3 targetPos)
    {
        _actorMoveSystem.MoveTo(targetPos);
        _actorAnimation.PlayAnim("Run");
        _actorSight.ResetLookInSight();
    }

    float IStateControl.GetAnimationClipLength(string name) => _actorAnimation.GetStateLength(name);

    bool IStateControl.IsTargetPosArrival() => _actorMoveSystem.IsArrivalTargetPos();

    SightableObject IStateControl.GetInSightAvailableMovingTarget()
    {
        // 視界に捉えたものが部屋の入口だった場合はそこに移動して良いかどうかを確認する
        SightableObject inSightObject = _actorSight.CurrentInSightObject;
        if (inSightObject?.SightableType == SightableType.RoomEntrance)
        {
            // 移動可能な場合はそのオブジェクトを、不可能な場合はnullを返す
            if (_actorMoveSystem.IsWaypointAvailable(inSightObject.transform.position))
            {
                return inSightObject;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return inSightObject;
        }
    }

    void IStateControl.ToggleSight(bool isActive)
    {
        if (isActive)
        {
            _actorSight.StartLookInSight();
        }
        else
        {
            _actorSight.StopLookInSight();
        }
    }
}
