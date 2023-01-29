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
    readonly int AppearAnimState = Animator.StringToHash("Slash");
    readonly int PanicAnimState = Animator.StringToHash("Slash");

    [SerializeField] Animator _anim;
    [SerializeField] ActorMove _actorMove;
    [Header("System�I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;
    [SerializeField] GameObject _testDestroyedPrefab;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    bool _isPlayAnim;
    bool _isTransitionable;

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
            if (state.StateInfo.IsName("Sla!sh"))
            {
                // Slash�̃A�j���[�V�����̃X�e�[�g�ɓ�������
                // ������g�����Ƃ��S�O���Ȃ��ł��������I
            }

        }).AddTo(this);
    }

    public void MoveToTarget()
    {
        _isTransitionable = false;
        _actorMove.MoveFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void RunToTarget()
    {
        _isTransitionable = false;
        _actorMove.RunFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void CancelMoveToTarget() => _actorMove.MoveCancel();

    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    public bool IsTransitionable() => _isTransitionable;

    public void PlayWanderAnim()
    {
        _isTransitionable = false;
        _actorMove.LookAround(() => _isTransitionable = true);
    }

    // ���v���t�@�N�^�����O
    public void PlayAppearAnim()
    {
        // TODO:�D��x(��) �A�j���[�V�������𕶎���Ŏw�肵�Ă���̂�Hash�ɒ���
        _anim.Play(AppearAnimState);
        _isTransitionable = false;
        // 2�b��Ƀt���O��܂��Ă��邪������A�j���[�V�����ɍ��킹�鏈�����K�v
        DOVirtual.DelayedCall(2.0f, () => _isTransitionable = true);
    }

    public bool IsTransitionToPanicState()
    {
        // ������������
        return Input.GetKeyDown(KeyCode.W);
    }

    // ���v���t�@�N�^�����O
    public void PlayPanicAnim()
    {
        // ���������Ƃ��̃A�j���[�V�������Đ�
        _anim.Play(PanicAnimState);
        _isTransitionable = false;
        // 2�b��Ƀt���O��܂��Ă��邪������A�j���[�V�����ɍ��킹�鏈�����K�v
        DOVirtual.DelayedCall(2.0f, () => _isTransitionable = true);
        // �Ώۂ̕����Ɍ�����(MoveState)
    }

    public bool IsTransitionToDeadState()
    {
        // �����S�𔻒肷��
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Instantiate(_testDestroyedPrefab, transform.position, Quaternion.identity);
    }
}
