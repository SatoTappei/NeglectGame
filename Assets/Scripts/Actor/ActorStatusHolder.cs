using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃X�e�[�^�X��ێ�����R���|�[�l���g
/// </summary>
public class ActorStatusHolder : MonoBehaviour
{
    [SerializeField] ActorStatusSO _actorStatusSO;

    public Sprite Icon => _actorStatusSO.Icon;
    public int MaxHp => _actorStatusSO.MaxHp;

    public string GetLineWithState(StateType type) => _actorStatusSO.GetLineWithState(type);
}
