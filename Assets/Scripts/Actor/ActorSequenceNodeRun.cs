using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p����w�肵���ʒu�ւ̈ړ����s���m�[�h
/// </summary>
public class ActorSequenceNodeRun : ActorSequenceNodeBase
{
    protected override async UniTask BehaviorAsync(CancellationTokenSource cts)
    {
        Debug.Log("�ړ����܂�");
        await UniTask.Delay(System.TimeSpan.FromSeconds(2.0f), cancellationToken: cts.Token);
        Debug.Log("�ړ�����efz");
    }
}