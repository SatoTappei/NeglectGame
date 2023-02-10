/// <summary>
/// UniRxのMessageBroker機能を用いてDamageReceiverコンポーネントから送信されるデータ
/// </summary>
public struct AttackDamageData
{
    public AttackDamageData(bool isDead)
    {
        IsDead = isDead;
    }

    public bool IsDead { get; }
}
