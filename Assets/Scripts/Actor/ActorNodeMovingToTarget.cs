using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 視界に捉えたオブジェクトの位置への移動を行うノード
/// </summary>
public class ActorNodeMovingToTarget : ActorNodeBase
{
    public ActorNodeMovingToTarget(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        _stateMachine.StateControl.MoveToNoSight(inSightObject);
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsTargetPosArrival(), 
            cancellationToken: cts.Token);
    }
}