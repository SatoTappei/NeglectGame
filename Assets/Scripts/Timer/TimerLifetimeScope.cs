using MessagePipe;
using VContainer;
using VContainer.Unity;

/// <summary>
/// インゲーム中に使用するタイマーのメッセージングが行えるように
/// MessagePipeを登録するためのLifetimeScope
/// </summary>
public class TimerLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        MessagePipeOptions options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<InGameTimerAddValue>(options);
    }
}
