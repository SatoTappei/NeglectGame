using UnityEngine;

/// <summary>
/// UIに表示するキャラクターのステータスのSO
/// </summary>
[CreateAssetMenu(fileName = "ActorStatus_")]
public class ActorStatusSO : ScriptableObject
{
    [Header("UIへ表示するアイコン")]
    [SerializeField] Sprite _icon;
    [Header("登場時の台詞")]
    [SerializeField] string _appearLine;

    public Sprite Icon { get => _icon; set => _icon = value; }
    public string AppearLine { get => _appearLine; set => _appearLine = value; }
}
