using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

/// <summary>
/// 各ノードを順に実行するクラス
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

    public async UniTask ExecuteAsync(CancellationToken token, UnityAction callback)
    {
        try
        {
            token.ThrowIfCancellationRequested();

            foreach (ActorNodeBase node in _sequence)
            {
                await node.PlayAsync(token);
            }

            callback?.Invoke();
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("Sequenceの処理がキャンセルされました: " + e.Message);
        }
    }
}
