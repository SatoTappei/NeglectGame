using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorMove : MonoBehaviour
{
    readonly float DashMag = 3.5f;

    [Header("�ړ����x")]
    [SerializeField] float _speed;

    // �ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�
    CancellationTokenSource _token;

    public void MoveFollowPath(Stack<Vector3> stack, bool isDash, UnityAction callBack)
    {
        // TODO:����͓s�x�g�[�N����new���Ă���̂ő��̕��@���������͍�����
        _token = new CancellationTokenSource();
        MoveAsync(stack, isDash, callBack).Forget();
    }

    // �_�b�V���̃t���O�����������A�_�b�V���ƕ����̃��\�b�h������ă��b�v����
    // TODO:�ړ���DOTween�ōs�������V���v���ɂȂ邩������Ȃ�
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
