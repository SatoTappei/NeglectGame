/// <summary>
/// UniRx��MessageBroker�@�\��p����DamageReceiver�R���|�[�l���g���瑗�M�����f�[�^
/// </summary>
public struct AttackDamageData
{
    public AttackDamageData(bool isDead)
    {
        IsDead = isDead;
    }

    public bool IsDead { get; }
}
