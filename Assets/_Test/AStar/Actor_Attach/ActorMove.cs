using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorMove : MonoBehaviour
{
    readonly float DashMag = 1.5f;

    [Header("�ړ����x")]
    [SerializeField] float _speed;

    // �ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�
    CancellationTokenSource _token;

    public void MoveFollowPath(Stack<Vector3> stack, bool isDash)
    {
        // TODO:����͓s�x�g�[�N����new���Ă���̂ő��̕��@���������͍�����
        _token = new CancellationTokenSource();
        MoveAsync(stack, isDash).Forget();
    }

    // TODO:�ړ���DOTween�ōs�������V���v���ɂȂ邩������Ȃ�
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
