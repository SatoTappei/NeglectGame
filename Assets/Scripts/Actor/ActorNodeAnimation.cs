using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用するアニメーションの再生を行うノード
/// </summary>
public class ActorNodeAnimation : ActorNodeBase
{
    string _animationName;

    public ActorNodeAnimation(ActorStateMachine stateMachine, ActorStateSequence sequence, string animationName)
        : base(stateMachine, sequence)
    {
        _animationName = animationName;
    }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        _stateMachine.StateControl.PlayAnimation(_animationName);
        float delay = _stateMachine.StateControl.GetAnimationClipLength(_animationName);
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: cts.Token);
    }
}