using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ���E�ɑ������I�u�W�F�N�g�̈ʒu�ւ̈ړ����s���m�[�h
/// </summary>
public class ActorNodeMoveToTarget : ActorNodeBase
{
    public ActorNodeMoveToTarget(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        _stateMachine.StateControl.RunToInactiveLookInSight(inSightObject);

        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsTargetPosArrival(), 
            cancellationToken: cts.Token);
    }
}