using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用する各振る舞いの基底クラス
/// </summary>
public abstract class ActorSequenceNodeBase
{
    public async UniTask PlayAsync(CancellationTokenSource cts)
    {
        await BehaviorAsync(cts);
    }

    protected abstract UniTask BehaviorAsync(CancellationTokenSource cts);
}
