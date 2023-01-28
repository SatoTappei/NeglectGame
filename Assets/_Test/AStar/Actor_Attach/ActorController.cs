using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] Animator _anim;
    [SerializeField] ActorMove _actorMove;
    [Header("System�I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    //bool _isInit;
    bool _isPlayAnim;

    void Start()
    {
        // TODO:���̎Q�Ɛ�̎擾���@���Ȃ񂩋C�ɂȂ�
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        ObservableStateMachineTrigger trigger =
            _anim.GetBehaviour<ObservableStateMachineTrigger>();

        trigger.OnStateEnterAsObservable().Subscribe(state =>
        {
            AnimatorStateInfo info = state.StateInfo;
            if (info.IsName("Sla!sh"))
            {
                // Slash�̃A�j���[�V�����̃X�e�[�g�ɓ�������
                // ������g�����Ƃ��S�O���Ȃ��ł��������I
            }

        }).AddTo(this);
    }

    public bool IsTransitionIdleState()
    {
        // �e�X�g
        // �A�j���[�V�����̍Đ��I����AIdle�X�e�[�g�ɑJ�ڂ���
        return !_isPlayAnim; 
    }

    public void MoveToTarget(bool isDash)
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _actorMove.MoveFollowPath(pathStack, isDash);
    }

    public bool IsTransitionMoveState()
    {
        
        return !_isPlayAnim;
    }

    public void MoveCancel()
    {
        _actorMove.MoveCancel();
    }

    public bool IsTransitionAnimationState()
    {
        return false;
        //if (_isInit)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    public void PlayAnim()
    {
        // TODO:�D��x(��) �A�j���[�V�������𕶎���Ŏw�肵�Ă���̂�Hash�ɒ���
        _anim.Play("Slash");
        _isPlayAnim = true;
        DOVirtual.DelayedCall(2.0f, () => _isPlayAnim = false);
    }
}
