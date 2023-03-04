using UnityEngine;
using UniRx;

/// <summary>
/// キャラクターのHPを制御するコンポーネント
/// </summary>
public class ActorHpControl : MonoBehaviour
{
    [Header("体力の減少量(秒)")]
    [SerializeField] int _decreaseQuantity = 1;
    [Header("体力の減少間隔")]
    [SerializeField] float _decreaseDuration = 0.5f;
    [Header("体力が少ないと判定される閾値")]
    [SerializeField] int _hpThreshold = 50;

    int _maxHp;
    ReactiveProperty<int> _currentHp = new();
    
    public IReadOnlyReactiveProperty<int> CurrentHp => _currentHp;

    public void InitOnStart(int maxHp)
    {
        _maxHp = maxHp;
        _currentHp.Value = maxHp;
    }

    internal void StartDecreaseHpPerSecond() => InvokeRepeating(nameof(DecreaseHpPerSecond), 0, _decreaseDuration);
    internal void StopDecreaseHpPerSecond() => CancelInvoke(nameof(DecreaseHpPerSecond));
    internal bool IsBelowHpThreshold() => _currentHp.Value < _hpThreshold;
    internal bool IsHpEqualZero() => _currentHp.Value <= 0;

    void DecreaseHpPerSecond() => DecreaseHp(_decreaseQuantity);

    internal void DecreaseHp(int quantity)
    {
        _currentHp.Value = Mathf.Clamp(_currentHp.Value -= quantity, 0, _maxHp);
    }

    public void Damage(int quantity)
    {
        DecreaseHp(quantity);

        if (IsHpEqualZero())
        {
            AudioManager.Instance.PlaySE("SE_死亡");
        }
        else
        {
            AudioManager.Instance.PlaySE("SE_ダメージ");
        }
    }
}
