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

    public async UniTask PlayAsync(CancellationToken token)
    {
        await ExecuteAsync(token);
    }

    protected abstract UniTask ExecuteAsync(CancellationToken token);
}
