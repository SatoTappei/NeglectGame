using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用する各ノードの基底クラス
/// </summary>
public abstract class ActorNodeBase
{
    protected ActorStateMachine _stateMachine;

    public ActorNodeBase(ActorStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async UniTask PlayAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();
        await ExecuteAsync(cts);
    }

    protected abstract UniTask ExecuteAsync(CancellationTokenSource cts);
}
