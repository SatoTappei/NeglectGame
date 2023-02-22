using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e�R���|�[�l���g�𐧌䂷��R���|�[�l���g
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
        // ���E�ɑ��������̂������̓����������ꍇ�͂����Ɉړ����ėǂ����ǂ������m�F����
        SightableObject inSightObject = _actorSight.CurrentInSightObject;
        if (inSightObject?.SightableType == SightableType.RoomEntrance)
        {
            // �ړ��\�ȏꍇ�͂��̃I�u�W�F�N�g���A�s�\�ȏꍇ��null��Ԃ�
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
