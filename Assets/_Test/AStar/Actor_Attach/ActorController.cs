using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    /* 
     *  �I���W�i���̋K��ɏ�����ALSystem�̂ق���������x���t�@�N�^�����O���� 
     */

    // �U���X�e�[�g�̋���
    // �L�����N�^�[�ƓG���A�^�b�N���[�V�������J��Ԃ�
    // �^�[�����ł͂Ȃ�
    // �G�̍U�����[�V�������q�b�g���邽�т�HP������Ă���
    // �G���|�ꂽ�Ƃ���������ǂ����邩
    //  ���S�����ۂɃ��b�Z�[�W���O������
    //  ���b�Z�[�W���O���Ȃ��ꍇ�A�R���C�_�[�ɓ��������L�����N�^�[�̍U���L�����Z���������ĂԕK�v������
    //  �ʂ̃L�����Ɏ蕿������肳�ꂽ�ꍇ��Move�X�e�[�g�ɖ߂��Ă܂����낤�낷�邵���Ȃ��H
    //  ����������l���������ł��Ȃ��悤�ɂ���ׂ�
    //  ��������l�ɂ���K�v������
    //  �����̔����SightableObject�R���|�[�l���g�łǂ��ɂ��ł�����
    //  �G���Ƀ_���[�W��^���鏈���������K�v�͂Ȃ�(�A�j���[�V�����̃^�C�~���O�ŃL������HP�����点�΂���������)
    //  ���ł��ǂ���������Ȃ�

    // ���݂̏�Ԃ̌��_:�ǂ̃X�e�[�g�ɂǂꂪ�g���Ă��邩���͂����肵�Ȃ�
    //                  ��������̓C���^�[�t�F�[�X���ǂ����Ɉˑ����Ă��邩�̖��

    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    ActorControlHelper _actorControlHelper;
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    // ���̃X�e�[�g�����߂�(���̃X�e�[�g�Ɉڂ�ۂ�StateID��Non�ɂ���悤�ɒ����ׂ�)
    StateID _nextState;
    /// <summary>
    /// �e�X�e�[�g�ɑJ�ڂ�������false�ɂȂ�A���̃X�e�[�g�̍s�����I�������
    /// true�ɂȂ��ăX�e�[�g����̑J�ډ\�ɂȂ�
    /// </summary>
    bool _isTransitionable;
    /// <summary>���̃t���O��true�ɂȂ����ꍇ�A���͕K��Escape�X�e�[�g�ɑJ�ڂ���</summary>
    bool _isPurposeCompleted;

    void Awake()
    {
        _actorControlHelper = new ActorControlHelper();
    }

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _nextState = StateID.Non;

        // MessageBroker�œG��|�������b�Z�[�W���󂯎��e�X�g�p
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => _isPurposeCompleted = true);
    }

    void IStateControl.PlayAnim(StateID current, StateID next)
    {
        string stateName = _actorControlHelper.StateIDToString(current);

        _isTransitionable = false;
        _actorAnimation.PlayAnim(stateName, () => 
        {
            _isTransitionable = true;
            _nextState = next;
        });
    }

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    // ����ړ��ɂ����̃t���O���g�p����Ă���̂ŁAPlayAnim���\�b�h�����ł��Ȃ�Move�X�e�[�g��
    // �ړ����������^�C�~���O�ł��̃��\�b�h��true��Ԃ��悤�ɂȂ�
    bool IStateControl.IsTransitionable() => _isTransitionable;

    void IStateControl.CancelAnim(string name)
    {
        // �A�j���[�V�����L�����Z������
    }

    bool IStateControl.IsDead() => _actorHpControl.IsHpEqualZero();

    bool IStateControl.IsTargetLost()
    {
        // ���b�Z�[�W�̎�M�͐�p�̃��b�Z�[�WReceiver������Ă������Ŏ󂯎��

        // �U�����Ă���Ώۂ��|�ꂽ��true
        if (_isPurposeCompleted)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    void IStateControl.ExploreRandom()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () => 
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject;

        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        Vector3 targetPos = inSightObject.transform.position;
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        _actorAction.RunFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.MoveToExit()
    {
        Vector3 targetPos = _pathfindingTarget.GetExitPos();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.CancelMoveToTarget()
    {
        _actorAction.MoveCancel();
    }

    bool IStateControl.IsSightTarget()
    {
        // TODO:���̃L������������ԂɂȂ�Ȃ��悤�ɑΏۂ̔����\�t���O��؂�ւ���K�v������
        if (_actorSight.IsFindInSight())
        {
            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.EffectAround() => _actorEffecter.EffectAround();
}