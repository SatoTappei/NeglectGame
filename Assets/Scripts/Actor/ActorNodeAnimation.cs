using Cysharp.Threading.Tasks;
using System.Threading;
using System;

/// <summary>
/// ActorStateSequenceクラスで使用するアニメーションの再生を行うノード
/// </summary>
public class ActorNodeAnimation : ActorNodeBase
{
    string _animationName;
    int _iteration;

    public ActorNodeAnimation(ActorStateMachine stateMachine,  string animationName, int iteration = 1)
        : base(stateMachine)
    {
        _animationName = animationName;
        _iteration = iteration;
    }

    protected override async UniTask ExecuteAsync(CancellationTokenSource cts)
    {
        // 指定した回数分同じアニメーションを繰り返す
        for(int i = 0; i < _iteration; i++)
        {
            cts.Token.ThrowIfCancellationRequested();

            _stateMachine.StateControl.PlayAnimation(_animationName);
            Debug.Log((i + 1) + "かいくりかえしたです");
            float delay = _stateMachine.StateControl.GetAnimationClipLength(_animationName);
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cts.Token);
        }
    }
}