/// <summary>
/// UniRxのMessageBroker機能を用いてDamageReceiverコンポーネントから送信されるデータ
/// </summary>
public struct DamageData
{
    public DamageData(bool isDead)
    {
        IsDead = isDead;
    }

    public bool IsDead { get; }
}
