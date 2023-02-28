using Cysharp.Threading.Tasks;
using System.Threading;

public class ActorNodeAffect : ActorNodeBase
{
    string _message;

    public ActorNodeAffect(ActorStateMachine stateMachine, string message) : base(stateMachine)
    {
        _message = message;
    }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        _stateMachine.StateControl.AffectAroundEffectableObject(_message);
        await UniTask.Yield();
    }
}
