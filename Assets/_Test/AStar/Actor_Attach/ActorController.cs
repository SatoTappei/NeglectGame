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

    // �e�X�e�[�g�̑J�ڏ����̐ݒ芮���A�J�ڏ����Ɏg���e���\�b�h�̃��t�@�N�^�����O������

    readonly string SystemObjectTag = "GameController";
    [SerializeField] ActorAction _actorAction;
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

    void Update()
    {
        // �e�X�g�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _nextState = StateID.Dead;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _nextState = StateID.Panic;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _nextState = StateID.Attack;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            _nextState = StateID.Joy;
        }
    }

    void IStateControl.PlayAnim(string name)
    {
        _isTransitionable = false;

        switch (name)
        {
            case "Appear":
                _actorAction.PlayAppearAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Move;
                });
                break;
            case "LookAround":
                _actorAction.PlayLookAroundAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Move;
                });
                break;
            case "Panic":
                _actorAction.PlayPanicAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Run;
                });
                break;
            case "Attack":
                _actorAction.PlayAttackAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Non;
                });
                break;
            case "Joy":
                _actorAction.PlayJoyAnim(() =>
                {
                    _isTransitionable = true;
                    _nextState = StateID.Non;
                });
                break;
        }
    }

    bool IStateControl.IsEqualNextState(StateID state) => _nextState == state;

    // ����ړ��ɂ����̃t���O���g�p����Ă���̂ŁAPlayAnim���\�b�h�����ł��Ȃ�Move�X�e�[�g��
    // �ړ����������^�C�~���O�ł��̃��\�b�h��true��Ԃ��悤�ɂȂ�
    bool IStateControl.IsTransitionable() => _isTransitionable;


    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    void IStateControl.CancelAnim(string name)
    {
        // �A�j���[�V�����L�����Z������
    }

    bool IStateControl.IsDead()
    {
        // ���S������true�ɂȂ�
        return false;
    }

    bool IStateControl.IsTargetLost()
    {
        // �^�[�Q�b�g���X�g
        return false;
    }

    void IStateControl.MoveToTarget()
    {
        _isTransitionable = false;
        _actorAction.MoveFollowPath(GetPathStack(), () => 
        {
            _isTransitionable = true;
            _nextState = StateID.LookAround;
        });
    }

    void IStateControl.RunToTarget()
    {
        _isTransitionable = false;

        // ���̕��򏈗��A���������̂ŕ��򂷂�
        // ���������̂Ɍ������đ���悤�ɂȂ��Ă���

        if (_findedTreasure != null)
        {
            if (_findedTreasure.name == "Enemy")
            {
                _nextState = StateID.Attack;
            }
            else
            {
                _nextState = StateID.Joy;
            }
        }

        var pos = _findedTreasure.transform.position;

        _actorAction.RunFollowPath(_pathGetable.GetPathStack(transform.position, pos), () =>
        {
            _isTransitionable = true;
        });
    }

    void IStateControl.CancelMoveToTarget()
    {
        _actorAction.MoveCancel();
    }

    bool IStateControl.IsSightTarget()
    {
        GameObject go = _actorSight.GetInSightObject();
        if (go != null)
        {
            _findedTreasure = go;
        }
        // ���t���[���Ă΂�Ă���̂�0.3�b�����Ƃ��ɌĂ΂��悤�ɂ���

        if (_actorSight.IsFindTreasure())
        {
            _nextState = StateID.Panic;
            return true;
        }

        return false;
    }

    void IStateControl.MoveToExit()
    {
        Debug.Log("�o����");
        // �o���Ɍ������Ĉړ����鏈��
    }
}