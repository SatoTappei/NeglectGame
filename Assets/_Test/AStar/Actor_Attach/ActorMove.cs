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
    readonly float DashMag = 3.5f;

    [Header("移動速度")]
    [SerializeField] float _speed;

    // 移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ
    CancellationTokenSource _token;

    public void MoveFollowPath(Stack<Vector3> stack, bool isDash, UnityAction callBack)
    {
        // TODO:現状は都度トークンをnewしているので他の方法が無いか模索する
        _token = new CancellationTokenSource();
        MoveAsync(stack, isDash, callBack).Forget();
    }

    // ダッシュのフラグを持たせず、ダッシュと歩きのメソッドを作ってラップする
    // TODO:移動はDOTweenで行う方がシンプルになるかもしれない
    async UniTaskVoid MoveAsync(Stack<Vector3> stack, bool isDash, UnityAction callBack)
    {
        foreach (Vector3 pos in stack)
        {
            while (true)
            {
                if (transform.position == pos) break;

                float speed = _speed * (isDash ? DashMag : 1);
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: _token.Token);
            }
        }

        callBack.Invoke();
    }

    public void LookAround(UnityAction callback)
    {
        int iteration = 1;
        int dir = UnityEngine.Random.Range(0, 2) == 1 ? 90 : -90;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(new Vector3(0, dir, 0), 1f)
                                 .SetRelative()
                                 .SetDelay(0.5f)
                                 .SetEase(Ease.InOutSine))
                                 .SetLink(gameObject);
        sequence.SetLoops(iteration, LoopType.Yoyo);
        sequence.OnComplete(() => callback?.Invoke());
    }

    public void MoveCancel() => _token?.Cancel();

    private void OnDestroy()
    {
        _token?.Cancel();
    }
}
