/// <summary>
/// UniRx��MessageBroker�@�\��p����DamageReceiver�R���|�[�l���g���瑗�M�����f�[�^
/// </summary>
public struct DamageData
{
    public DamageData(bool isDead)
    {
        IsDead = isDead;
    }

    public bool IsDead { get; }
}
