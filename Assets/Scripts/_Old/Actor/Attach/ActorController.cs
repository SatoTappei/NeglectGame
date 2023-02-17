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

    [SerializeField] ActorPathfindingMove _actorAction;
    [SerializeField] ActorAnimation _actorAnimation;
    [SerializeField] ActorHpControl _actorHpControl;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;
    [SerializeField] DamagedMessageReceiver _damagedMessageReceiver;

    ActorControlHelper _actorControlHelper = new ActorControlHelper();
    ITargetSelectable _targetSelectable;
    IPathfinding _pathGetable;
    StateID _nextState = StateID.Non;
    /// <summary>
    /// �e�X�e�[�g�ɑJ�ڂ�������false�ɂȂ�A���̃X�e�[�g�̍s�����I�������
    /// true�ɂȂ��ăX�e�[�g����̑J�ډ\�ɂȂ�
    /// </summary>
    bool _isTransitionable;
    /// <summary>���̃t���O��true�ɂȂ����ꍇ�A���͕K��Escape�X�e�[�g�ɑJ�ڂ���</summary>
    bool _isCompleted;

    /*
     * ����邱��
     * �p�[�c�͈�ʂ蓮�����̂͊��������̂Ń����_���������Ȃ��n�`�ň�ʂ�Q�[���ɂȂ邩�����B
     * �EInGameStream�����A�Q�[���S�J�̗����ǂ��Ȃ�����s
     * �E�ȈՓI��UI/���o���Ȃ�������n�߂�
     * �E�I�u�W�F�N�g�̃��|�b�v�@�\
     * �EInGameStream�����̂ŃV�[���S�̂̊Ǘ����\�ɂȂ�
     * �E�e��R���|�[�l���g�̎蒼��������
     */

    // �ǂ�ȃQ�[���H
    // �_���W�����ɓ����Ă���`���҂�㩂Ŏז�����Q�[��
    // �v���C���[�̓_���W������㩂�u����
    // �`���҂�㩂𓥂ނƃ_���[�W
    // ��������̖`���҂𑒂낤

    void Awake()
    {
        _damagedMessageReceiver.OnDefeated += () => _isCompleted = true;
    }

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _targetSelectable = system.GetComponent<ITargetSelectable>();
        _pathGetable = system.GetComponent<IPathfinding>();
    }

    void IStateControl.PlayAnim(StateID current, StateID next)
    {
        string stateName = _actorControlHelper.StateIDToString(current);

        // �A�j���[�V�����̍Đ������������^�C�~���O��isTransitionable��true�ɂȂ邱�Ƃ�
        // _nextState�ւ̑J�ڂ��\�ɂȂ�
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
        MoveTo(_targetSelectable.GetNextWaypointPos(), _actorAction.MoveFollowPath);
    }

    void IStateControl.RunToTarget()
    {
        GameObject inSightObject = _actorSight.CurrentInSightObject.gameObject;
        SightableType target = inSightObject.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        MoveTo(inSightObject.transform.position, _actorAction.RunFollowPath);
    }

    void IStateControl.MoveToExit()
    {
        _nextState = StateID.LookAround;
        MoveTo(_targetSelectable.GetExitPos(), _actorAction.MoveFollowPath);
    }

    void MoveTo(Vector3 targetPos, UnityAction<Stack<Vector3>, UnityAction> moveMethod)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathToWaypoint(transform.position, targetPos);

        // �^�[�Q�b�g�ւ̈ړ������������^�C�~���O��isTransitionable��true�ɂȂ邱�Ƃ�
        // _nextState�ւ̑J�ڂ��\�ɂȂ�
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