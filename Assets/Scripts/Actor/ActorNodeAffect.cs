using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p�������̕��ɉe�����y�ڂ����s���m�[�h
/// </summary>
public class ActorNodeAffect : ActorNodeBase
{
    string _message;

    public ActorNodeAffect(ActorStateMachine stateMachine, string message) : base(stateMachine)
    {
        _message = message;
    }

    protected override async UniTask ActionAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        _stateMachine.StateControl.AffectAroundEffectableObject(_message);
        await UniTask.Yield(cancellationToken: cts.Token);
    }
}
