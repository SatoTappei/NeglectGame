using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 視界に捉えたオブジェクトの位置への移動を行うノード
/// </summary>
public class ActorNodeRunToInSightObject : ActorNodeBase
{
    public ActorNodeRunToInSightObject(ActorStateMachine stateMachine, ActorStateSequence sequence)
        : base(stateMachine, sequence)
    {

    }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        _stateMachine.StateControl.MoveTo(inSightObject.transform.position);
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsArrivalTargetPos(), cancellationToken: cts.Token);
    }
}