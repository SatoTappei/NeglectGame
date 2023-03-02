using UnityEngine;

/// <summary>
/// UI�ɕ\������L�����N�^�[�̃X�e�[�^�X��SO
/// </summary>
[CreateAssetMenu(fileName = "ActorStatus_")]
public class ActorStatusSO : ScriptableObject
{
    [Header("UI�֕\������A�C�R��")]
    [SerializeField] Sprite _icon;
    [Header("�ő�HP")]
    [SerializeField] int _maxHp = 100;
    [Header("�o�ꎞ�̑䎌")]
    [SerializeField] string _appearLine;

    public Sprite Icon => _icon;
    public int MaxHp => _maxHp;
    public string AppearLine => _appearLine;
}
