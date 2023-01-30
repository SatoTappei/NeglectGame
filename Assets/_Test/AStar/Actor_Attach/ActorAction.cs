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
/// 経路探索をした結果を用いてキャラクターを移動させるコンポーネント
/// </summary>
public class ActorAction : MonoBehaviour
{
    readonly int WalkAnimState = Animator.StringToHash("Walk");
    readonly int SprintAnimState = Animator.StringToHash("Sprint");
    readonly int LookAroundAnimState = Animator.StringToHash("LookAround");
    readonly int AppearAnimState = Animator.StringToHash("Appear");
    readonly int PanicAnimState = Animator.StringToHash("Jump");

    [SerializeField] Animator _anim;
    [Header("移動速度")]
    [SerializeField] float _speed;
    [Header("ダッシュ時の速度倍率")]
    [SerializeField] float _dashMag = 1.5f;
    // テスト
    [SerializeField] AnimationClip _look;
    [SerializeField] AnimationClip _appear;
    [SerializeField] AnimationClip _panic;


    // 移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ
    CancellationTokenSource _token;

    void Start()
    {
        // これつかってない
        //ObservableStateMachineTrigger trigger =
        //    _anim.GetBehaviour<ObservableStateMachineTrigger>();

        //trigger.OnStateEnterAsObservable().Subscribe(state =>
        //{
        //    if (state.StateInfo.IsName("Sla!sh"))
        //    {
        //        // Slashのアニメーションのステートに入った時
        //        // これを使うことを躊躇しないでください！
        //    }

        //}).AddTo(this);
    }

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        // TODO:現状は都度トークンをnewしているので他の方法が無いか模索する
        _token = new CancellationTokenSource();
        _anim.Play(WalkAnimState);
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        // TODO:現状は都度トークンをnewしているので他の方法が無いか模索する
        _token = new CancellationTokenSource();
        _anim.Play(SprintAnimState);
        MoveAsync(stack, _speed * _dashMag, callBack).Forget();
    }

    // TODO:移動はDOTweenで行う方がシンプルになるかもしれない
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

// ここで回転させるなら子のModelの方を回転しないといけない
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