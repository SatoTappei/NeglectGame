using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �L�����N�^�[�̓����X�e�[�^�X��\���R���|�[�l���g
/// </summary>
public class ActorStatus : MonoBehaviour
{
    [Header("�̗͂̍ő�l")]
    [SerializeField] int _maxHp;
    [Header("�̗͂̌�����(�b)")]
    [SerializeField] int _decreaseQuantity;
    [Header("�̗͂����Ȃ��Ɣ��肳���臒l")]
    [SerializeField] int _HpThreshold;
    // ���ӗ~�̕\���̃e�X�g�p
    [SerializeField] Text _testText;

    int _currentHp;

    void Awake()
    {
        _currentHp = _maxHp;
    }

    void Start()
    {
        InvokeRepeating(nameof(DecreaseMotivation), 0, 0.1f);
    }

    internal bool IsBelowMotivationThreshold() => _currentHp < _HpThreshold;

    void DecreaseMotivation()
    {
        // TODO:0�ȉ��ɂȂ��Ă��܂��̂ŏC��
        _currentHp = Mathf.Clamp(_currentHp -= _decreaseQuantity, 0, _maxHp);

        // �e�X�g�p��UI�ɕ\��
        _testText.text = _currentHp.ToString();
    }   
}
