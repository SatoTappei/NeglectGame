using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    readonly string SystemObjectTag = "GameController";

    [SerializeField] ActorMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    ActorControlHelper _actorControlHelper = new ActorControlHelper();
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;
    StateID _nextState = StateID.Non;
    /// <summary>
    /// �e�X�e�[�g�ɑJ�ڂ�������false�ɂȂ�A���̃X�e�[�g�̍s�����I�������
    /// true�ɂȂ��ăX�e�[�g����̑J�ډ\�ɂȂ�
    /// </summary>
    bool _isTransitionable;
    /// <summary>���̃t���O��true�ɂȂ����ꍇ�A���͕K��Escape�X�e�[�g�ɑJ�ڂ���</summary>
    bool _isCompleted;

    // �D��^�X�N
    // Move�̏�����isTransitionable�ϐ��ňꊇ�ŊǗ����Ă����̂��l����
    // �G��|�������̃��b�Z�[�W���O�������ǂ��ɂ�����
    // �퓬����̂Ń_���[�W���󂯂鏈�������
    // �I�u�W�F�N�g�̃��|�b�v�@�\
    // �E�o�̍ۂ̉��o
    // �����_���������Ȃ��n�`�ň�ʂ�Q�[���ɂȂ邩����
    
    // �ǂ�ȃQ�[���H
    // �_���W�����ɓ����Ă���`���҂�㩂Ŏז�����Q�[��
    // �v���C���[�̓_���W������㩂�u����
    // �`���҂�㩂𓥂ނƃ_���[�W
    // ��������̖`���҂𑒂낤

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        // MessageBroker�œG��|�������b�Z�[�W���󂯎��e�X�g�p
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => _isCompleted = true);
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

    void IStateControl.MoveToRandomWaypoint()
    {
        _nextState = StateID.LookAround;
        Move(_pathfindingTarget.GetPathfindingTarget(), _actorAction.MoveFollowPath);
    }

    void IStateControl.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject.gameObject;
        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        Move(inSightObject.transform.position, _actorAction.RunFollowPath);
    }

    void IStateControl.MoveToExit()
    {
        _nextState = StateID.LookAround;
        Move(_pathfindingTarget.GetExitPos(), _actorAction.MoveFollowPath);
    }

    void Move(Vector3 targetPos, UnityAction<Stack<Vector3>, UnityAction> moveMethod)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);

        _isTransitionable = false;
        moveMethod(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.CancelMoving() => _actorAction.MoveCancel();

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    bool IStateControl.IsTransitionable() => _isTransitionable;

    bool IStateControl.IsDead() => _actorHpControl.IsHpEqualZero();

    bool IStateControl.IsCompleted()
    {
        if (_isCompleted)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    bool IStateControl.IsSightTarget()
    {
        if (_actorSight.IsFindInSight())
        {
            SightableObject inSightObject = _actorSight.CurrentInSightObject;
            
            if (inSightObject.HasWitness()) return false;
            inSightObject.SetWitness(gameObject);

            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.EffectAroundEffectableObject() => _actorEffecter.EffectAround();
}