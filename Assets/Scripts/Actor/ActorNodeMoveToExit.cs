using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 出口に向かって移動するノード
/// </summary>
public class ActorNodeMoveToExit : ActorNodeBase
{
    public ActorNodeMoveToExit(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override async UniTask ActionAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        _stateMachine.StateControl.MoveToExit();
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsTargetPosArrival(), 
            cancellationToken: cts.Token);
    }
}
