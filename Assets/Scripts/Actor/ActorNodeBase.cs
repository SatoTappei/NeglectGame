using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用する各ノードの基底クラス
/// </summary>
public abstract class ActorNodeBase
{
    protected ActorStateMachine _stateMachine;
    protected ActorStateSequence _sequence;

    public ActorNodeBase(ActorStateMachine stateMachine, ActorStateSequence sequence)
    {
        _stateMachine = stateMachine;
        _sequence = sequence;
    }

    public async UniTask PlayAsync(CancellationTokenSource cts)
    {
        await ExecuteAsync(cts);
    }

    protected abstract UniTask ExecuteAsync(CancellationTokenSource cts);
}
