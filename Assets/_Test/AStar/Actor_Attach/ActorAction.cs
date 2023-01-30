using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorAction : MonoBehaviour
{
    readonly int WalkAnimState = Animator.StringToHash("Walk");
    readonly int SprintAnimState = Animator.StringToHash("Sprint");
    readonly int LookAroundAnimState = Animator.StringToHash("LookAround");
    readonly int AppearAnimState = Animator.StringToHash("Appear");
    readonly int PanicAnimState = Animator.StringToHash("Jump");

    [SerializeField] Animator _anim;
    [Header("�ړ����x")]
    [SerializeField] float _speed;
    [Header("�_�b�V�����̑��x�{��")]
    [SerializeField] float _dashMag = 1.5f;
    // �e�X�g
    [SerializeField] AnimationClip _look;
    [SerializeField] AnimationClip _appear;
    [SerializeField] AnimationClip _panic;


    // �ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�
    CancellationTokenSource _token;

    void Start()
    {
        // ��������ĂȂ�
        //ObservableStateMachineTrigger trigger =
        //    _anim.GetBehaviour<ObservableStateMachineTrigger>();

        //trigger.OnStateEnterAsObservable().Subscribe(state =>
        //{
        //    if (state.StateInfo.IsName("Sla!sh"))
        //    {
        //        // Slash�̃A�j���[�V�����̃X�e�[�g�ɓ�������
        //        // ������g�����Ƃ��S�O���Ȃ��ł��������I
        //    }

        //}).AddTo(this);
    }

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        // TODO:����͓s�x�g�[�N����new���Ă���̂ő��̕��@���������͍�����
        _token = new CancellationTokenSource();
        _anim.Play(WalkAnimState);
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        // TODO:����͓s�x�g�[�N����new���Ă���̂ő��̕��@���������͍�����
        _token = new CancellationTokenSource();
        _anim.Play(SprintAnimState);
        MoveAsync(stack, _speed * _dashMag, callBack).Forget();
    }

    // TODO:�ړ���DOTween�ōs�������V���v���ɂȂ邩������Ȃ�
    async UniTaskVoid MoveAsync(Stack<Vector3> stack, float speed, UnityAction callBack)
    {
        foreach (Vector3 pos in stack)
        {
            while (true)
            {
                if (transform.position == pos) break;

                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: _token.Token);
            }
        }

        callBack.Invoke();
    }

    internal void MoveCancel() => _token?.Cancel();

    internal void LookAround(UnityAction callback)
    {
        //Debug.Log(_anim.GetCurrentAnimatorClipInfo(0).Length);
        //Debug.Log(_anim.GetCurrentAnimatorStateInfo(0).length);
        //Debug.Log(_anim.GetNextAnimatorStateInfo(0));
        //_anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //_anim.Play(LookAroundAnimState);
        //DOVirtual.DelayedCall(3.5f, () => callback?.Invoke());
        //Hoge(LookAroundAnimState, callback).Forget();
        PlayAnim(LookAroundAnimState, callback, _look);
    }

    internal void PlayAppearAnim(UnityAction callback)
    {
        //Debug.Log(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //Debug.Log(_anim.GetNextAnimatorStateInfo(0));
        //_anim.Play(AppearAnimState);
        //DOVirtual.DelayedCall(2.0f, () => callback?.Invoke());
        //Hoge(AppearAnimState, callback).Forget();
        PlayAnim(AppearAnimState, callback, _appear);
    }

    internal void PlayPanicAnim(UnityAction callback)
    {
        //Debug.Log(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //Debug.Log(_anim.GetNextAnimatorStateInfo(0));
        //_anim.Play(PanicAnimState);
        //DOVirtual.DelayedCall(2.0f, () => callback?.Invoke());
        //Hoge(PanicAnimState, callback).Forget();
        PlayAnim(PanicAnimState, callback, _panic);
    }

    void PlayAnim(int hash, UnityAction callback, AnimationClip clip)
    {
        _anim.Play(hash);
        DOVirtual.DelayedCall(clip.length, () => callback?.Invoke());
    }

    async UniTaskVoid Hoge(int hash, UnityAction callback)
    {
        _anim.Play(hash);
        //await UniTask.Yield();
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        callback.Invoke();
    }

    void OnDestroy()
    {
        _token?.Cancel();
    }
}

// �����ŉ�]������Ȃ�q��Model�̕�����]���Ȃ��Ƃ����Ȃ�
//int iteration = 1;
//int dir = UnityEngine.Random.Range(0, 2) == 1 ? 90 : -90;

//Sequence sequence = DOTween.Sequence();
//sequence.Append(transform.DORotate(new Vector3(0, dir, 0), 1f)
//                         .SetRelative()
//                         .SetDelay(0.5f)
//                         .SetEase(Ease.InOutSine))
//                         .SetLink(gameObject);
//sequence.SetLoops(iteration, LoopType.Yoyo);
//sequence.OnComplete(() => callback?.Invoke());