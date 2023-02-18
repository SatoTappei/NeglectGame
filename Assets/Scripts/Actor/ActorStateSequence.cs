using System;
using System.Threading;

/// <summary>
/// �eActorSequenceNode�����Ɏ��s����N���X
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
            Debug.Log("Sequence�̏������L�����Z������܂���: " + e.Message);
        }

        Debug.Log("Sequence�I��");
    }
}
