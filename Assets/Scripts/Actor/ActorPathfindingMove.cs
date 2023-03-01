using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������N���X
/// </summary>
public class ActorPathfindingMove
{
    /// <summary>�ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�</summary>
    CancellationTokenSource _cts;
    /// <summary>transform���g���Ȃ��̂ő���Ɏ��g���Q�Ƃ�����</summary>
    GameObject _actor;
    Transform _actorTransform;
    /// <summary>�i�s��������������Model�I�u�W�F�N�g</summary>
    Transform _model;

    float _moveSpeed;
    float _runSpeedMag;

    public ActorPathfindingMove(GameObject actor, Transform model, float moveSpeed, float runSpeedMag)
    {
        _actor = actor;
        _actorTransform = actor.transform;
        _model = model;
        _moveSpeed = moveSpeed;
        _runSpeedMag = runSpeedMag;
    }

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _cts = new CancellationTokenSource();
        MoveFollowPathAsync(stack, _moveSpeed, callBack, _cts).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _cts = new CancellationTokenSource();
        MoveFollowPathAsync(stack, _moveSpeed * _runSpeedMag, callBack, _cts).Forget();
    }

    async UniTaskVoid MoveFollowPathAsync(Stack<Vector3> stack, float speed, UnityAction callBack, 
        CancellationTokenSource cts)
    {
        _cts.Token.ThrowIfCancellationRequested();
        _cts = cts;

        await MoveAsync(stack, speed, _cts);
        callBack?.Invoke();
    }

    async UniTask MoveAsync(Stack<Vector3> stack, float speed, CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        foreach (Vector3 nextPos in stack)
        {
            _model.DOLookAt(nextPos, 0.5f).SetLink(_actor);

            // GameObject���j�����ꂽ�Ƃ���null���o��̂�null�`�F�b�N���K�v
            while (_actor != null && _actorTransform.position != nextPos)
            {
                if (_actor == null)
                {
                    cts.Cancel();
                    return;
                }

                _actorTransform.position = Vector3.MoveTowards(_actorTransform.position, nextPos, 
                    Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: cts.Token);
            }
        }
    }

    internal void MoveCancel() => _cts?.Cancel();
}