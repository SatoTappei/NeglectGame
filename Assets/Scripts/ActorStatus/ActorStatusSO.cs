using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UI�ɕ\������L�����N�^�[�̃X�e�[�^�X��SO
/// </summary>
[CreateAssetMenu(fileName = "ActorStatus_")]
public class ActorStatusSO : ScriptableObject
{
    /// <summary>
    /// �X�e�[�g�ɉ������䎌�̃f�[�^
    /// </summary>
    [Serializable]
    struct LineData
    {
        [SerializeField] StateType _type;
        [SerializeField] string _line;

        public StateType Type => _type;
        public string Line => _line;
    }

    [Header("UI�֕\������A�C�R��")]
    [SerializeField] Sprite _icon;
    [Header("�ő�HP")]
    [SerializeField] int _maxHp = 100;
    [Header("�X�e�[�g�ɉ������䎌(�e�X�e�[�g�ɑ΂���1�̂�)")]
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
            //Debug.LogWarning("�Ή�����䎌������܂���" + type);
            return string.Empty;
        }
    }
}
