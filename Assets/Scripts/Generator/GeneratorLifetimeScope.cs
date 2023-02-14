using MessagePipe;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 外部からジェネレータを停止/再開を行うメッセージの送受信をするために
/// MessagePipeを登録するためのLifetimeScope
/// </summary>
public class GeneratorLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MessagePipeOptions options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<GeneratorControl>(options);
    }
}
