using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターのHPを制御するコンポーネント
/// </summary>
public class ActorHpControl : MonoBehaviour
{
    [Header("体力の最大値")]
    [SerializeField] int _maxHp;
    [Header("体力の減少量(秒)")]
    [SerializeField] int _decreaseQuantity;
    [Header("体力が少ないと判定される閾値")]
    [SerializeField] int _HpThreshold;
    // ★意欲の表示のテスト用
    [SerializeField] Text _testText;

    //int _currentHp;
    ActorStatus _actorStatus;
    // 扱っている値がActorStatusの値に依存しないことを明確にするためにプロパティでラップしておく
    int CurrentHp { get => _actorStatus.Hp; set => _actorStatus.Hp = value; }

    internal void Init(ActorStatus actorStatus)
    {
        _actorStatus = actorStatus;
        CurrentHp = _maxHp;
    }

    internal void StartDecreaseHpPerSecond() => InvokeRepeating(nameof(DecreaseHp), 0, 0.1f);
    internal void StopDecreaseHpPerSecond() { /* TODO:毎秒のHP減少を止める処理 */ }
    internal bool IsBelowHpThreshold() => CurrentHp < _HpThreshold;
    internal bool IsHpIsZero() => CurrentHp <= 0;

    void DecreaseHp()
    {
        CurrentHp = Mathf.Clamp(CurrentHp -= _decreaseQuantity, 0, _maxHp);

        // テスト用にUIに表示
        _testText.text = CurrentHp.ToString();
    }   
}
