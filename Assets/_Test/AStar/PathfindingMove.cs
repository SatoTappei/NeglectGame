using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class PathfindingMove : MonoBehaviour
{
    [SerializeField] float _speed;

    public void Move(Stack<Vector3> stack)
    {
        MoveAsync(stack).Forget();
    }

    // TODO:�ړ���DOTween�ōs�������V���v���ɂȂ邩������Ȃ�
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
