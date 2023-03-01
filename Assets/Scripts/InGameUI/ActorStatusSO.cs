using UnityEngine;

/// <summary>
/// UI�ɕ\������L�����N�^�[�̃X�e�[�^�X��SO
/// </summary>
[CreateAssetMenu(fileName = "ActorStatus_")]
public class ActorStatusSO : ScriptableObject
{
    [Header("UI�֕\������A�C�R��")]
    [SerializeField] Sprite _icon;
    [Header("�o�ꎞ�̑䎌")]
    [SerializeField] string _appearLine;

    public Sprite Icon { get => _icon; set => _icon = value; }
    public string AppearLine { get => _appearLine; set => _appearLine = value; }
}
