using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p����e�U�镑���̊��N���X
/// </summary>
public abstract class ActorSequenceNodeBase
{
    public async UniTask PlayAsync(CancellationTokenSource cts)
    {
        await BehaviorAsync(cts);
    }

    protected abstract UniTask BehaviorAsync(CancellationTokenSource cts);
}
