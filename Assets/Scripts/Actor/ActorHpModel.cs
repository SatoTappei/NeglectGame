using UnityEngine;
using UniRx;

/// <summary>
/// �L�����N�^�[��HP�𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorHpModel : MonoBehaviour
{
    [Header("�̗͂̍ő�l")]
    [SerializeField] int _maxHp = 100;
    [Header("�̗͂̌�����(�b)")]
    [SerializeField] int _decreaseQuantity = 1;
    [Header("�̗͂̌����Ԋu")]
    [SerializeField] float _decreaseDuration = 0.5f;
    [Header("�̗͂����Ȃ��Ɣ��肳���臒l")]
    [SerializeField] int _hpThreshold = 50;

    ReactiveProperty<int> _currentHp = new();
    public IReadOnlyReactiveProperty<int> CurrentHp => _currentHp;

    public void Init()
    {
        _currentHp.Value = _maxHp;
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
}
