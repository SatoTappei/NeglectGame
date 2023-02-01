using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターの内部ステータスを表すコンポーネント
/// </summary>
public class ActorStatus : MonoBehaviour
{
    [Header("体力の最大値")]
    [SerializeField] int _maxHp;
    [Header("体力の減少量(秒)")]
    [SerializeField] int _decreaseQuantity;
    [Header("体力が少ないと判定される閾値")]
    [SerializeField] int _HpThreshold;
    // ★意欲の表示のテスト用
    [SerializeField] Text _testText;

    int _currentHp;

    void Awake()
    {
        _currentHp = _maxHp;
    }

    void Start()
    {
        InvokeRepeating(nameof(DecreaseHp), 0, 0.1f);
    }

    internal bool IsBelowHpThreshold() => _currentHp < _HpThreshold;
    internal bool IsHpIsZero() => _currentHp <= 0;

    void DecreaseHp()
    {
        _currentHp = Mathf.Clamp(_currentHp -= _decreaseQuantity, 0, _maxHp);

        // テスト用にUIに表示
        _testText.text = _currentHp.ToString();
    }   
}
