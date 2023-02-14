using MessagePipe;
using VContainer;
using VContainer.Unity;

/// <summary>
/// �O������W�F�l���[�^���~/�ĊJ���s�����b�Z�[�W�̑���M�����邽�߂�
/// MessagePipe��o�^���邽�߂�LifetimeScope
/// </summary>
public class GeneratorLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MessagePipeOptions options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<GeneratorControl>(options);
    }
}
