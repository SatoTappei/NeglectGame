using System;
using System.Threading;

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
            Debug.Log("Sequence�̏������L�����Z������܂���: " + e.Message);
        }

        Debug.Log("Sequence�I��");
    }
}
