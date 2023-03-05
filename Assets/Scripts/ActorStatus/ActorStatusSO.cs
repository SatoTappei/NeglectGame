using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UIに表示するキャラクターのステータスのSO
/// </summary>
[CreateAssetMenu(fileName = "ActorStatus_")]
public class ActorStatusSO : ScriptableObject
{
    /// <summary>
    /// ステートに応じた台詞のデータ
    /// </summary>
    [Serializable]
    struct LineData
    {
        [SerializeField] StateType _type;
        [SerializeField] string _line;

        public StateType Type => _type;
        public string Line => _line;
    }

    [Header("UIへ表示するアイコン")]
    [SerializeField] Sprite _icon;
    [Header("最大HP")]
    [SerializeField] int _maxHp = 100;
    [Header("ステートに応じた台詞(各ステートに対して1つのみ)")]
    [SerializeField] LineData[] _lineDatas;

    Dictionary<StateType, string> _lineDic = new(Enum.GetValues(typeof(StateType)).Length);

    public Sprite Icon => _icon;
    public int MaxHp => _maxHp;

    void OnEnable()
    {
        _lineDic = _lineDatas.ToDictionary(l => l.Type, l => l.Line);
    }

    public string GetLineWithState(StateType type)
    {
        if (_lineDic.TryGetValue(type, out string line))
        {
            return line;
        }
        else
        {
            //Debug.LogWarning("対応する台詞がありません" + type);
            return string.Empty;
        }
    }
}
