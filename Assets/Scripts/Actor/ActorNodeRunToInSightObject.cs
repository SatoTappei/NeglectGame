using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ���E�ɑ������I�u�W�F�N�g�̈ʒu�ւ̈ړ����s���m�[�h
/// </summary>
public class ActorNodeRunToInSightObject : ActorNodeBase
{
    public ActorNodeRunToInSightObject(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override async UniTask ExecuteAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        _stateMachine.StateControl.MoveTo(inSightObject.transform.position);
        await UniTask.WaitUntil(() => _stateMachine.StateControl.IsTargetPosArrival(), cancellationToken: token);
    }
}