using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// oŒû‚ÉŒü‚©‚Á‚ÄˆÚ“®‚·‚éƒm[ƒh
/// </summary>
public class ActorNodeMoveToExit : ActorNodeBase
{
    public ActorNodeMoveToExit(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        _stateMachine.StateControl.MoveToExit();
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsTargetPosArrival(), 
            cancellationToken: cts.Token);
    }
}
