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
    [SerializeField] ActorDisappearPerformance _actorDisappearPerformance;

    ActorInSightFilter _actorInSightFilter;

    /* 
     *  ���E�̎��������������̂ł�����Ƃ������E�ɂȂ�悤�ɒ���
     *  ���O�h�[�����Ԃ����Ō����o��悤�ɂ���
     *  �_���W�����ɕ���u��
     *  �̗͂�0�ȉ��ɂȂ����玀�ʂ悤�ɂ���
     */

    void Awake()
    {
        _actorInSightFilter = new();
    }

    /* 
     *  Awake()��OnEnable()�̌�AStart()�̒��O�ɊO���ňʒu�����������Ă��� 
     */

    void Start()
    {
        _actorAnimation.Init();
        _actorMoveSystem.Init();
        _actorStateMachine.Init();

        // Update�Ƃ͕ʂ̃^�C�~���O�A�����ŌĂ΂��
        _actorSight.StartLookInSight();
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
    }

    void IStateControl.PlayDeadPerformance()
    {
        _actorDisappearPerformance.PlayDeadPerformance();
        _actorMoveSystem.MoveCancel();
        _actorSight.StopLookInSight();
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
            // �ړ���Ƃ��Ďg����I�u�W�F�N�g���n���ꂽ�ꍇ�A�ړ����n�߂�܂Ŏ��E�̋@�\���~�߂Ă���
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