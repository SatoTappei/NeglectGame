using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 経路探索をした結果を用いてキャラクターを移動させるクラス
/// </summary>
public class ActorPathfindingMove
{
    /// <summary>移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ</summary>
    CancellationTokenSource _cts;
    /// <summary>transformが使えないので代わりに自身を参照させる</summary>
    GameObject _actor;
    /// <summary>進行方向を向かせるModelオブジェクト</summary>
    Transform _model;

    float _moveSpeed;
    float _runSpeedMag;

    public ActorPathfindingMove(GameObject actor, Transform model, float moveSpeed, float runSpeedMag)
    {
        _actor = actor;
        _model = model;
        _moveSpeed = moveSpeed;
        _runSpeedMag = runSpeedMag;
    }

    internal async UniTaskVoid MoveFollowPathAsync(Stack<Vector3> stack, UnityAction callBack)
    {
        _cts = new CancellationTokenSource();
        await MoveAsync(stack, _moveSpeed, _cts);
        callBack?.Invoke();
    }

    internal async UniTaskVoid RunFollowPathAsync(Stack<Vector3> stack, UnityAction callBack)
    {
        _cts = new CancellationTokenSource();
        await MoveAsync(stack, _moveSpeed * _runSpeedMag, _cts);
        callBack?.Invoke();
    }

    async UniTask MoveAsync(Stack<Vector3> stack, float speed, CancellationTokenSource cts)
    {
        foreach (Vector3 nextPos in stack)
        {
            _model.DOLookAt(nextPos, 0.5f).SetLink(_actor);

            // GameObjectが破棄されたときにnullが出るのでnullチェックが必要
            while (_actor != null && _actor.transform.position != nextPos)
            {
                if (_actor == null)
                {
                    cts.Cancel();
                    return;
                }

                _actor.transform.position = Vector3.MoveTowards(_actor.transform.position, nextPos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: cts.Token);
            }
        }
    }

    internal void MoveCancel() => _cts?.Cancel();
}