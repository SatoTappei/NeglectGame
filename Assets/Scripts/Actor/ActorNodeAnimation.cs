using Cysharp.Threading.Tasks;
using System.Threading;
using System;

/// <summary>
/// ActorStateSequence�N���X�Ŏg�p����A�j���[�V�����̍Đ����s���m�[�h
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
        // �w�肵���񐔕������A�j���[�V�������J��Ԃ�
        for(int i = 0; i < _iteration; i++)
        {
            cts.Token.ThrowIfCancellationRequested();

            _stateMachine.StateControl.PlayAnimation(_animationName);
            Debug.Log((i + 1) + "�������肩�������ł�");
            float delay = _stateMachine.StateControl.GetAnimationClipLength(_animationName);
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cts.Token);
        }
    }
}