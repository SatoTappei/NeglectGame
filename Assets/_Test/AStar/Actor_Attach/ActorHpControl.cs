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
    // ���ӗ~�̕\���̃e�X�g�p
    [SerializeField] Text _testText;

    int _currentHp;

    void Awake()
    {
        _currentHp = _maxHp;
    }

    internal void StartDecreaseHpPerSecond() => InvokeRepeating(nameof(DecreaseHp), 0, _decreaseDuration);
    internal void StopDecreaseHpPerSecond() { /* TODO:���b��HP�������~�߂鏈�� */ }
    internal bool IsBelowHpThreshold() => _currentHp < _HpThreshold;
    internal bool IsHpEqualZero() => _currentHp <= 0;

    void DecreaseHp()
    {
        _currentHp = Mathf.Clamp(_currentHp -= _decreaseQuantity, 0, _maxHp);

        // �e�X�g�p��UI�ɕ\��
        _testText.text = _currentHp.ToString();
    }   
}
