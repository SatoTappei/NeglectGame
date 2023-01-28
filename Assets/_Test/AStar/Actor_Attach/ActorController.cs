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
    [SerializeField] GameObject _testDestroyedPrefab;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    //bool _isInit;
    bool _isPlayAnim;
    bool _isPlayDiscoverAnim;
    bool _isMoving;
    bool _isLookArounding;

    void Start()
    {
        // TODO:���̎Q�Ɛ�̎擾���@���Ȃ񂩋C�ɂȂ�
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        // ��������ĂȂ�
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
        _actorMove.MoveFollowPath(pathStack, isDash, () => _isMoving = false);
        _isMoving = true;
    }

    public bool IsTransitionToWanderStateFromMoveState()
    {
        // �ړ����I��������Ƃ����m���Ă��낤��ɑJ�ڂ�����
        return !_isMoving;
    }

    public void MoveCancel()
    {
        _actorMove.MoveCancel();
    }

    public void PlayLookAround()
    {
        _actorMove.LookAround(() => _isLookArounding = false);
        _isLookArounding = true;
    }

    public bool IsTransitionToMoveStateFromWanderStateAfterLookAroundDOtweenAnimation()
    {
        return !_isLookArounding;
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
        // 2�b��Ƀt���O��܂��Ă��邪������A�j���[�V�����ɍ��킹�鏈�����K�v
        DOVirtual.DelayedCall(2.0f, () => _isPlayAnim = false);
    }

    public bool IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDiscoverAnim()
    {
        // ���������Ƃ��̃A�j���[�V�������Đ�
        _anim.Play("Slash");
        _isPlayDiscoverAnim = true;
        // 2�b��Ƀt���O��܂��Ă��邪������A�j���[�V�����ɍ��킹�鏈�����K�v
        DOVirtual.DelayedCall(2.0f, () => _isPlayDiscoverAnim = false);
        // �Ώۂ̕����Ɍ�����(MoveState)
    }

    public void FromAnyStateDead()
    {
        Destroy(gameObject);
        Instantiate(_testDestroyedPrefab, transform.position, Quaternion.identity);
    }

    public bool IsTransitionToAnimationStateFromMoveState()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool IsTransitionToMoveStateFromDiscoverState()
    {
        return !_isPlayDiscoverAnim;
    }
}
