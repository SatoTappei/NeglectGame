using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p����w�肵���ʒu�ւ̈ړ����s���m�[�h
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