using System;
using System.Threading;

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

    public async void Execute(CancellationTokenSource cts)
    {
        try
        {
            foreach (ActorNodeBase node in _sequence)
            {
                await node.PlayAsync(cts);
            }
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("Sequenceの処理がキャンセルされました: " + e.Message);
        }

        Debug.Log("Sequence終了");
    }
}
