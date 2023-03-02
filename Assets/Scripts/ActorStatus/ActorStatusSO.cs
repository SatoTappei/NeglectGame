using UnityEngine;

/// <summary>
/// UIに表示するキャラクターのステータスのSO
/// </summary>
[CreateAssetMenu(fileName = "ActorStatus_")]
public class ActorStatusSO : ScriptableObject
{
    [Header("UIへ表示するアイコン")]
    [SerializeField] Sprite _icon;
    [Header("最大HP")]
    [SerializeField] int _maxHp = 100;
    [Header("登場時の台詞")]
    [SerializeField] string _appearLine;

    public Sprite Icon => _icon;
    public int MaxHp => _maxHp;
    public string AppearLine => _appearLine;
}
