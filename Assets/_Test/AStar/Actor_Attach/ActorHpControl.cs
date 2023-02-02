using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �L�����N�^�[��HP�𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorHpControl : MonoBehaviour
{
    [Header("�̗͂̍ő�l")]
    [SerializeField] int _maxHp;
    [Header("�̗͂̌�����(�b)")]
    [SerializeField] int _decreaseQuantity;
    [Header("�̗͂����Ȃ��Ɣ��肳���臒l")]
    [SerializeField] int _HpThreshold;
    // ���ӗ~�̕\���̃e�X�g�p
    [SerializeField] Text _testText;

    //int _currentHp;
    ActorStatus _actorStatus;
    // �����Ă���l��ActorStatus�̒l�Ɉˑ����Ȃ����Ƃ𖾊m�ɂ��邽�߂Ƀv���p�e�B�Ń��b�v���Ă���
    int CurrentHp { get => _actorStatus.Hp; set => _actorStatus.Hp = value; }

    internal void Init(ActorStatus actorStatus)
    {
        _actorStatus = actorStatus;
        CurrentHp = _maxHp;
    }

    internal void StartDecreaseHpPerSecond() => InvokeRepeating(nameof(DecreaseHp), 0, 0.1f);
    internal void StopDecreaseHpPerSecond() { /* TODO:���b��HP�������~�߂鏈�� */ }
    internal bool IsBelowHpThreshold() => CurrentHp < _HpThreshold;
    internal bool IsHpIsZero() => CurrentHp <= 0;

    void DecreaseHp()
    {
        CurrentHp = Mathf.Clamp(CurrentHp -= _decreaseQuantity, 0, _maxHp);

        // �e�X�g�p��UI�ɕ\��
        _testText.text = CurrentHp.ToString();
    }   
}
