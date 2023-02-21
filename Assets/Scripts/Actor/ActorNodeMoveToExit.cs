using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// oŒû‚ÉŒü‚©‚Á‚ÄˆÚ“®‚·‚éƒm[ƒh
/// </summary>
public class ActorNodeMoveToExit : ActorNodeBase
{
    public ActorNodeMoveToExit(ActorStateMachine stateMachine, ActorStateSequence sequence)
        : base(stateMachine, sequence)
    {

    }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        _stateMachine.StateControl.MoveToExit();
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsArrivalTargetPos(), cancellationToken: cts.Token);
    }
}
