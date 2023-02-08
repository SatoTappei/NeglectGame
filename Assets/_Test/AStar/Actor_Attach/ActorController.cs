using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IStateControl
{
    /* 
     *  �I���W�i���̋K��ɏ�����ALSystem�̂ق���������x���t�@�N�^�����O���� 
     */

    // 

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

    void Start()
    {
        GameObject system = GameObject.FindGameObjectWithTag(SystemObjectTag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        _nextState = StateID.Non;
        _actorStatus = new ActorStatus();
        _actorHpControl.Init(_actorStatus);
        _actorSight.Init(_actorStatus);
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
        // �^�[�Q�b�g���X�g
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