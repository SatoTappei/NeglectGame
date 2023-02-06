using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorMove : MonoBehaviour
{
    [SerializeField] ActorAnimation _actorAnimation;
    [Header("�i�s��������������Model�I�u�W�F�N�g")]
    [SerializeField] Transform _model;
    [Header("�ړ����x")]
    [SerializeField] float _speed = 2;
    [Header("�����Ĉړ����鎞�̑��x�{��")]
    [SerializeField] float _runSpeedMag = 2f;

    /// <summary>�ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�</summary>
    CancellationTokenSource _token;

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _actorAnimation?.PlayAnim("Move");
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _actorAnimation?.PlayAnim("Run");   
        MoveAsync(stack, _speed * _runSpeedMag, callBack).Forget();
    }

    internal void MoveCancel() => _token?.Cancel();

    async UniTaskVoid MoveAsync(Stack<Vector3> stack, float speed, UnityAction callBack)
    {
        _token = new CancellationTokenSource();

        foreach (Vector3 pos in stack)
        {
            _model.DOLookAt(pos, 0.5f).SetLink(gameObject);

            while (true)
            {
                if (transform.position == pos) break;

                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: _token.Token);
            }
        }

        callBack.Invoke();
    }

    void OnDestroy()
    {
        _token?.Cancel();
    }
}