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

    ActorInSightFilter _actorInSightFilter;

    void Awake()
    {
        _actorInSightFilter = new();
    }

    void Start()
    {
        _actorSight.StartLookInSight();
    }

    void Update()
    {
        //_actorSight.Execute();
        //_actorStateMachine.Execute();
    }

    /* 
     *  �ŗD��:�v���C���[�̃X�e�[�g�}�V����蒼�� 
     */

    // Sight�őS�Ă�SightableObject�𔭌�
    // ���t���[��SightableObject��null����Ȃ����Ď�
    // null�ȊO��������select�ɑJ��
    // SightableObject���Ƃɏ����𕪂���
    // �ړ�/Sequence

    // �ʘH��Waypoint�̓����_���ȉ���ł��������Ɉړ�����B�������A��������2�A���ł͈ړ����Ȃ�
    // ��x��������Treasure/Enemy/RoomEntrance�͓�x�Ɣ������Ȃ��B
    // 
    // ���E�̋@�\��On�ɂȂ��Ă���̂�Explore��MoveToEntrance��ԁA����ȊO�ł�off

    // Treasure/Enemy/RoomEntrance��Sight����擾�����
    // MoveSystem���ŊǗ����Ă������ Pass/RoomEntrance/Exit
    // ������Sight���̕����Ǘ�������̂͗ǂ��Ȃ��B
    // ��������~����
    // Sight�Ŕ��� => ��������ړ��\������ => Actor�̃C���^�t�F�[�X�o�R�ł��ĂƂ܂���ɓn��
    // ���Ϻ�

    // Sight�Ŕ��������I�u�W�F�N�g�͈ړ��Ɠ�����null�ɂ���ׂ��A�������邱�ƂŃX�e�[�g�̐؂�ւ��ŔY�܂Ȃ�
    // �����𔭌������ꍇ�͎��E�@�\��off�ɂ��Ă��܂�
    // �ړ����J�n�����Ɠ����ɍĂю��E�@�\��on�ɂ���ׂ��H

    // �ق���:MoveToNoSightable()�c�ړ��������E�̋@�\���g��Ȃ� = Sequence�p�̈ړ����\�b�h
    // 

    void IStateControl.PlayAnimation(string name) => _actorAnimation.PlayAnim(name);

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
        _actorSight.ResetLookInSight();
        _actorSight.StartLookInSight();
        _actorMoveSystem.MoveTo(target.transform.position);
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

    //void IStateControl.ToggleSight(bool isActive)
    //{
    //    if (isActive)
    //    {
    //        _actorSight.StartLookInSight();
    //    }
    //    else
    //    {
    //        _actorSight.StopLookInSight();
    //    }
    //}
}