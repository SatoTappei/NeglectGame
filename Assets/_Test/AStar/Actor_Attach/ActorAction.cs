using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// 経路探索をした結果を用いてキャラクターを移動させるコンポーネント
/// </summary>
public class ActorAction : MonoBehaviour
{
    // TODO: アニメーションの列挙型を作るとActorController側のリファクタリングが出来そう？
    //       適切に行えばActorController側ももう少しスッキリしそう
    //       そうするとステート名をreadonlyにすることが出来ない
    //       AnimationClip、ステート名(ハッシュ用)、呼び出しの列挙型を作ってひとまとめにするべき？  

    readonly int WalkAnimState = Animator.StringToHash("Walk");
    readonly int SprintAnimState = Animator.StringToHash("Sprint");
    readonly int LookAroundAnimState = Animator.StringToHash("LookAround");
    readonly int AppearAnimState = Animator.StringToHash("Appear");
    readonly int PanicAnimState = Animator.StringToHash("Jump");

    [SerializeField] Animator _anim;
    [Header("移動速度")]
    [SerializeField] float _speed;
    [Header("走って移動する時の速度倍率")]
    [SerializeField] float _runSpeedMag = 1.5f;
    [Header("アニメーションの長さ取得用")]
    [SerializeField] AnimationClip _lookAroundAnimClip;
    [SerializeField] AnimationClip _appearAnimClip;
    [SerializeField] AnimationClip _panicAnimClip;

    /// <summary>移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ</summary>
    CancellationTokenSource _token;

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _anim.Play(WalkAnimState);
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _anim.Play(SprintAnimState);   
        MoveAsync(stack, _speed * _runSpeedMag, callBack).Forget();
    }

    internal void MoveCancel() => _token?.Cancel();

    async UniTaskVoid MoveAsync(Stack<Vector3> stack, float speed, UnityAction callBack)
    {
        _token = new CancellationTokenSource();

        foreach (Vector3 pos in stack)
        {
            _anim.transform.DOLookAt(pos, 0.5f).SetLink(gameObject);

            while (true)
            {
                if (transform.position == pos) break;

                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: _token.Token);
            }
        }

        callBack.Invoke();
    }

    internal void PlayLookAroundAnim(UnityAction callback)
        => PlayAnim(LookAroundAnimState, callback, _lookAroundAnimClip);
    internal void PlayAppearAnim(UnityAction callback) 
        => PlayAnim(AppearAnimState, callback, _appearAnimClip);
    internal void PlayPanicAnim(UnityAction callback)
        => PlayAnim(PanicAnimState, callback, _panicAnimClip);

    void PlayAnim(int hash, UnityAction callback, AnimationClip clip)
    {
        // TODO: アニメーションの長さの取得をもっと綺麗にまとめたい
        //       現在は再生するアニメーションをインスペクターから割り当ててその長さ分だけ遅延させている

        // アニメーションする度にモデルの位置が少しずつズレていくので再生するたびに0に戻す処理を挟んでいる
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_anim.gameObject.transform.DOLocalMove(Vector3.zero, 0.15f))
                .AppendCallback(() => 
                {
                    _anim.Play(hash);
                    DOVirtual.DelayedCall(clip.length, () => callback?.Invoke());
                })
                .SetLink(gameObject);
    }

    void OnDestroy()
    {
        _token?.Cancel();
    }
}