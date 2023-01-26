using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorMove : MonoBehaviour
{
    readonly float DashMag = 1.5f;

    [Header("�ړ����x")]
    [SerializeField] float _speed;

    bool _isDash = true;

    public void MoveFollowPath(Stack<Vector3> stack)
    {
        MoveAsync(stack).Forget();
    }

    // TODO:�ړ���DOTween�ōs�������V���v���ɂȂ邩������Ȃ�
    async UniTaskVoid MoveAsync(Stack<Vector3> stack)
    {
        foreach (Vector3 pos in stack)
        {
            while (true)
            {
                if (transform.position == pos) break;

                float speed = _speed * (_isDash ? DashMag : 1);
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
            }
        }
    }
}
