using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Events;

/// <summary>
/// 各ノードを順に実行するクラス
/// </summary>
public class ActorStateSequence
{
    ActorNodeBase[] _sequence;
    int _addIndex;

    public ActorStateSequence(int length)
    {
        _sequence = new ActorNodeBase[length];
    }

    public void Add(ActorNodeBase node)
    {
        _sequence[_addIndex] = node;
        _addIndex++;
    }

    public async UniTask ExecuteAsync(CancellationTokenSource cts, UnityAction callback)
    {
        cts.Token.ThrowIfCancellationRequested();
        foreach (ActorNodeBase node in _sequence)
        {
            await node.ExecuteAsync(cts);
        }

        callback?.Invoke();
    }
}
