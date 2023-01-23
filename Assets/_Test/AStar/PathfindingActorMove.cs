using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 経路探索をした結果を用いてキャラクターを移動させるコンポーネント
/// </summary>
public class PathfindingActorMove : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _speed;

    public Transform Target { get => _target; set => _target = value; }

    public void Move(Stack<Vector3> stack)
    {
        MoveAsync(stack).Forget();
    }

    // TODO:移動はDOTweenで行う方がシンプルになるかもしれない
    async UniTaskVoid MoveAsync(Stack<Vector3> stack)
    {
        foreach (var v in stack)
        {
            while (true)
            {
                if (transform.position == v) break;

                transform.position = Vector3.MoveTowards(transform.position, v, Time.deltaTime * 10);
                await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }
    }
}
