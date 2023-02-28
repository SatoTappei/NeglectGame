using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] ActorHpModel _actorHpModel;

    ActorInSightFilter _actorInSightFilter;

    void Awake()
    {
        _actorInSightFilter = new();
    }

    /* 
     *  Awake()��OnEnable()�̌�AStart()�̒��O�ɊO���ňʒu�����������Ă��� 
     */

    /* 
     *  ���̃^�X�N 
     *  �_���W�������̑S�Ă̕�����������Ă������Ȃ��ꍇ�͋A��悤�ɂ���
     *  �G�Ƃ���̃��|�b�v
     */

    void Start()
    {
        _actorAnimation.Init();
        _actorMoveSystem.Init();
        _actorStateMachine.Init();
        _actorHpModel.Init();

        // Update�Ƃ͕ʂ̃^�C�~���O�A�����ŌĂ΂��
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
                    // �ړ���Ƃ��Ďg����I�u�W�F�N�g���n���ꂽ�ꍇ�A�ړ����n�߂�܂Ŏ��E�̋@�\���~�߂Ă���
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