using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用するアニメーションの再生を行うノード
/// </summary>
public class ActorSequenceNodeAnimation : ActorSequenceNodeBase
{
    protected override async UniTask BehaviorAsync(CancellationTokenSource cts)
    {
        Debug.Log("アニメーション再生");
        await UniTask.Delay(System.TimeSpan.FromSeconds(2.0f), cancellationToken: cts.Token);
        Debug.Log("アニメ完了qed");
    }
}