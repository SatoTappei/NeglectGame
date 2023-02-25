using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターのHPを制御するコンポーネント
/// </summary>
public class ActorHpModel : MonoBehaviour
{
    [Header("体力の最大値")]
    [SerializeField] int _maxHp = 100;
    [Header("体力の減少量(秒)")]
    [SerializeField] int _decreaseQuantity = 1;
    [Header("体力の減少間隔")]
    [SerializeField] float _decreaseDuration = 0.5f;
    [Header("体力が少ないと判定される閾値")]
    [SerializeField] int _hpThreshold = 50;

    int _currentHp;

    public void Init()
    {
        _currentHp = _maxHp;
    }

    internal void StartDecreaseHpPerSecond() => InvokeRepeating(nameof(DecreaseHpPerSecond), 0, _decreaseDuration);
    internal void StopDecreaseHpPerSecond() => CancelInvoke(nameof(DecreaseHpPerSecond));
    internal bool IsBelowHpThreshold() => _currentHp < _hpThreshold;
    internal bool IsHpEqualZero() => _currentHp <= 0;

    void DecreaseHpPerSecond() => DecreaseHp(_decreaseQuantity);

    internal void DecreaseHp(int quantity)
    {
        _currentHp = Mathf.Clamp(_currentHp -= quantity, 0, _maxHp);
        Debug.Log("現在:" + _currentHp);
    }   
}
