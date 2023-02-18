using System;
using System.Threading;

/// <summary>
/// 各ActorSequenceNodeを順に実行するクラス
/// </summary>
public class ActorStateSequence
{
    ActorSequenceNodeBase[] _sequence;
    int _insertIndex;

    public ActorStateSequence(int length)
    {
        _sequence = new ActorSequenceNodeBase[length];
    }

    public void Add(ActorSequenceNodeBase node)
    {
        _sequence[_insertIndex] = node;
        _insertIndex++;
    }

    public async void Play(CancellationTokenSource cts)
    {
        try
        {
            foreach (ActorSequenceNodeBase node in _sequence)
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
