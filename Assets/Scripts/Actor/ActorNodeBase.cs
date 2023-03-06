using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p����e�m�[�h�̊��N���X
/// </summary>
public abstract class ActorNodeBase
{
    protected ActorStateMachine _stateMachine;

    public ActorNodeBase(ActorStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        await ActionAsync(cts);
    }

    protected abstract UniTask ActionAsync(CancellationTokenSource cts);
}