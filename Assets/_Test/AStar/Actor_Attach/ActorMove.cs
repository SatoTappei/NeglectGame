using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// 経路探索をした結果を用いてキャラクターを移動させるコンポーネント
/// </summary>
public class ActorMove : MonoBehaviour
{
    readonly float DashMag = 1.5f;

    [Header("移動速度")]
    [SerializeField] float _speed;

    // 移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ
    CancellationTokenSource _token;

    public void MoveFollowPath(Stack<Vector3> stack, bool isDash)
    {
        // TODO:現状は都度トークンをnewしているので他の方法が無いか模索する
        _token = new CancellationTokenSource();
        MoveAsync(stack, isDash).Forget();
    }

    // TODO:移動はDOTweenで行う方がシンプルになるかもしれない
    async UniTaskVoid MoveAsync(Stack<Vector3> stack, bool isDash)
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
    }

    public void MoveCancel() => _token?.Cancel();
}
