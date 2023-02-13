using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �L�����N�^�[��HP�𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorHpControl : MonoBehaviour
{
    [Header("�̗͂̍ő�l")]
    [SerializeField] int _maxHp = 100;
    [Header("�̗͂̌�����(�b)")]
    [SerializeField] int _decreaseQuantity = 1;
    [Header("�̗͂̌����Ԋu")]
    [SerializeField] float _decreaseDuration = 0.5f;
    [Header("�̗͂����Ȃ��Ɣ��肳���臒l")]
    [SerializeField] int _HpThreshold = 50;

    int _currentHp;

    void Awake()
    {
        _currentHp = _maxHp;
    }

    void Update()
    {
        Debug.Log(gameObject.name + " :" + _currentHp);
    }

    internal void StartDecreaseHpPerSecond() => InvokeRepeating(nameof(DecreaseHpPerSecond), 0, _decreaseDuration);
    internal void StopDecreaseHpPerSecond() { /* TODO:���b��HP�������~�߂鏈�� */ }
    internal bool IsBelowHpThreshold() => _currentHp < _HpThreshold;
    internal bool IsHpEqualZero() => _currentHp <= 0;

    void DecreaseHpPerSecond() => DecreaseHp(_decreaseQuantity);

    internal void DecreaseHp(int quantity)
    {
        _currentHp = Mathf.Clamp(_currentHp -= quantity, 0, _maxHp);
    }   
}
