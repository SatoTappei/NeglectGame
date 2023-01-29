using System;
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
public class ActorMove : MonoBehaviour
{
    [SerializeField] Animator _anim;
    [Header("移動速度")]
    [SerializeField] float _speed;
    [Header("ダッシュ時の速度倍率")]
    [SerializeField] float _dashMag = 1.5f;

    // 移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ
    CancellationTokenSource _token;

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        // TODO:現状は都度トークンをnewしているので他の方法が無いか模索する
        _token = new CancellationTokenSource();
        _anim.Play("Walk");
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        // TODO:現状は都度トークンをnewしているので他の方法が無いか模索する
        _token = new CancellationTokenSource();
        _anim.Play("Sprint");
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

    public void LookAround(UnityAction callback)
    {

        _anim.Play("LookAround");
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

        DOVirtual.DelayedCall(3.5f, () => callback?.Invoke());
    }

    public void MoveCancel() => _token?.Cancel();

    private void OnDestroy()
    {
        _token?.Cancel();
    }
}
