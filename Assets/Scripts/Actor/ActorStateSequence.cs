using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

/// <summary>
/// �e�m�[�h�����Ɏ��s����N���X
/// </summary>
public class ActorStateSequence
{
    ActorNodeBase[] _sequence;
    int _insertIndex;

    public ActorStateSequence(int length)
    {
        _sequence = new ActorNodeBase[length];
    }

    public void Add(ActorNodeBase node)
    {
        _sequence[_insertIndex] = node;
        _insertIndex++;
    }

    public async UniTask ExecuteAsync(CancellationTokenSource cts, UnityAction callback)
    {
        try
        {
            cts.Token.ThrowIfCancellationRequested();

            foreach (ActorNodeBase node in _sequence)
            {
                await node.PlayAsync(cts);
            }

            callback?.Invoke();
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("Sequence�̏������L�����Z������܂���: " + e.Message);
        }
    }
}
