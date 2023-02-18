using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequenceクラスで使用する指定した位置への移動を行うノード
/// </summary>
public class ActorSequenceNodeRun : ActorSequenceNodeBase
{
    protected override async UniTask BehaviorAsync(CancellationTokenSource cts)
    {
        Debug.Log("移動します");
        await UniTask.Delay(System.TimeSpan.FromSeconds(2.0f), cancellationToken: cts.Token);
        Debug.Log("移動完了efz");
    }
}