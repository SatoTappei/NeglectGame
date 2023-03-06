using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用する周りの物に影響を及ぼすを行うノード
/// </summary>
public class ActorNodeAffect : ActorNodeBase
{
    string _message;

    public ActorNodeAffect(ActorStateMachine stateMachine, string message) : base(stateMachine)
    {
        _message = message;
    }

    protected override async UniTask ActionAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        _stateMachine.StateControl.AffectAroundEffectableObject(_message);
        await UniTask.Yield(cancellationToken: cts.Token);
    }
}
