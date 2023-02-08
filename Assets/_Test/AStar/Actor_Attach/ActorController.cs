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

    ActorStatus _actorStatus;
    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    /// <summary>
    /// �e�X�e�[�g�ɑJ�ڂ�������false�ɂȂ�
    /// ���̃X�e�[�g�̍s�����I�������true�ɂȂ��ăX�e�[�g����̑J�ډ\�ɂȂ�
    /// </summary>
    bool _isTransitionable;
    // ���̌���������(�G�܂�)�A�ʂ̃X�N���v�g�ɉf��
    GameObject _findedTreasure;

    // ���̃X�e�[�g�����߂�(���̃X�e�[�g�Ɉڂ�ۂ�StateID��Non�ɂ���悤�ɒ����ׂ�)
    StateID _nextState;

    // �_���W�����E�o�ړI�ł���G��|��������true�ɂȂ�
    bool isTargetDestroyed;

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _nextState = StateID.Non;
        _actorStatus = new ActorStatus();
        _actorHpControl.Init(_actorStatus);
        _actorSight.Init(_actorStatus);

        // MessageBroker�œG��|�������b�Z�[�W���󂯎��e�X�g�p
        MessageBroker.Default.Receive<AttackDamageData>().Subscribe(_ => isTargetDestroyed = true);
    }

    void IStateControl.PlayAnim(StateID current, StateID next)
    {
        string stateName = StateIDToString(current);

        // stateName���󂾂ƍĐ�����A�j���[�V�������Ȃ�
        if (stateName == string.Empty) return;

        _isTransitionable = false;
        _actorAnimation.PlayAnim(stateName, () => 
        {
            _isTransitionable = true;
            _nextState = next;
        });
    }

    string StateIDToString(StateID id)
    {
        switch (id)
        {
            case StateID.Appear:     return "Appear";
            case StateID.Attack:     return "Attack";
            case StateID.Joy:        return "Joy";
            case StateID.LookAround: return "LookAround";
            case StateID.Panic:      return "Panic";
        }

        Debug.LogError("�X�e�[�gID���o�^����Ă��܂���:" + id);
        return string.Empty;
    }

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    // ����ړ��ɂ����̃t���O���g�p����Ă���̂ŁAPlayAnim���\�b�h�����ł��Ȃ�Move�X�e�[�g��
    // �ړ����������^�C�~���O�ł��̃��\�b�h��true��Ԃ��悤�ɂȂ�
    bool IStateControl.IsTransitionable() => _isTransitionable;

    void IStateControl.CancelAnim(string name)
    {
        // �A�j���[�V�����L�����Z������
    }

    bool IStateControl.IsDead() => _actorHpControl.IsHpIsZero();

    bool IStateControl.IsTargetLost()
    {
        // �U�����Ă���Ώۂ��|�ꂽ��true
        if (isTargetDestroyed)
        {
            _nextState = StateID.Escape;
            return true;
        }

        return false;
    }

    void IStateControl.ExploreRandom()
    {
        Stack<Vector3> pathStack = GetPathStack(_pathfindingTarget.GetPathfindingTarget());
        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () => 
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.RunToTarget()
    {
        SightableType target = _findedTreasure.GetComponent<SightableObject>().SightableType;
        _nextState = target == SightableType.Enemy ? StateID.Attack : StateID.Joy;

        Stack<Vector3> pathStack = GetPathStack(_findedTreasure.transform.position);
        _isTransitionable = false;
        _actorAction.RunFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.MoveToExit()
    {
        Stack<Vector3> pathStack = GetPathStack(_pathfindingTarget.GetExitPos());
        _isTransitionable = false;
        _actorAction.MoveFollowPath(pathStack, () =>
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    Stack<Vector3> GetPathStack(Vector3 targetPos)
    {
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    void IStateControl.CancelMoveToTarget()
    {
        _actorAction.MoveCancel();
    }

    bool IStateControl.IsSightTarget()
    {
        GameObject InsightObject = _actorSight.GetInSightObject();
        if (InsightObject != null)
        {
            _findedTreasure = InsightObject;
            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }
}