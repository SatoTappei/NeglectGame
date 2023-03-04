using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// �L�����N�^�[�̊e�R���|�[�l���g�𐧌䂷��R���|�[�l���g
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
     *  Awake()��OnEnable()�̌�AStart()�̒��O�ɊO���ňʒu�����������Ă��� 
     */

    /* 
     *  ���̃^�X�N 
     *  �_���W�������̑S�Ă̕�����������Ă������Ȃ��ꍇ�͋A��悤�ɂ���
     *  �䎌�\���@�\
     *  ���̂ɃR���C�_�[���c���Ă���̂�㩂��������Ă��܂��A�̂Œ���
     *  ��
     *  �Q�[���Ƃ��Ă̑̍�(�^�C�g��=>�C���Q�[��=>�I��=>�����[�h)�𐮂���
     *  UI�̉摜�Ƃ�
     *  �Q�[���X�^�[�g�̉��o�̍쐬(�G���[�̌����ɂȂ�̂ŕK�{)
     */

    void Start()
    {
        _actorAnimation.InitOnStart();
        _actorMoveSystem.InitOnStart();
        _actorHpModel.InitOnStart(_actorStatusHolder.MaxHp);

        // Update�Ƃ͕ʂ̃^�C�~���O�A�����ŌĂ΂��
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
            // �ړ���Ƃ��Ďg����I�u�W�F�N�g���n���ꂽ�ꍇ�A�ړ����n�߂�܂Ŏ��E�̋@�\���~�߂Ă���
            _actorSight.StopLookInSight();
            return target;
        }
        else
        {
            return null;
        }
    }


}