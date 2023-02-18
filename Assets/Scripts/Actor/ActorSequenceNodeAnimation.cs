using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p����A�j���[�V�����̍Đ����s���m�[�h
/// </summary>
public class ActorSequenceNodeAnimation : ActorSequenceNodeBase
{
    protected override async UniTask BehaviorAsync(CancellationTokenSource cts)
    {
        Debug.Log("�A�j���[�V�����Đ�");
        await UniTask.Delay(System.TimeSpan.FromSeconds(2.0f), cancellationToken: cts.Token);
        Debug.Log("�A�j������qed");
    }
}