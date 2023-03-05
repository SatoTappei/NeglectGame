using UnityEngine;

/// <summary>
/// キャラクターのステータスを保持するコンポーネント
/// </summary>
public class ActorStatusHolder : MonoBehaviour
{
    [SerializeField] ActorStatusSO _actorStatusSO;

    public Sprite Icon => _actorStatusSO.Icon;
    public int MaxHp => _actorStatusSO.MaxHp;

    public string GetLineWithState(StateType type) => _actorStatusSO.GetLineWithState(type);
}
