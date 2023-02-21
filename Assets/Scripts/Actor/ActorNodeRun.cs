using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// ActorStateSequenceクラスで使用する指定した位置への移動を行うノード
/// </summary>
public class ActorNodeRun : ActorNodeBase
{
    Vector3 _targetPos;

    public ActorNodeRun(ActorStateMachine stateMachine, ActorStateSequence sequence)
        : base(stateMachine, sequence)
    {

    }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        _stateMachine.StateControl.MoveTo(_targetPos);
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsArrivalTargetPos(), cancellationToken: cts.Token);
    }
}