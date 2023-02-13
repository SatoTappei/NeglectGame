using MessagePipe;
using VContainer;
using VContainer.Unity;

/// <summary>
/// �C���Q�[�����Ɏg�p����^�C�}�[�̃��b�Z�[�W���O���s����悤��
/// MessagePipe��o�^���邽�߂�LifetimeScope
/// </summary>
public class TimerLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MessagePipeOptions options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<InGameTimerAddValue>(options);
    }
}
