using UniRx;
using UnityEngine;

/// <summary>
/// UniRxでデータを取り扱う際に必要な参照との仲介役になるコンポーネント
/// 未使用
/// </summary>
public class ActorRxMediator : MonoBehaviour
{
    [SerializeField] ActorHpControl _actorHpModel;
    [SerializeField] ActorStatusSO _actorStatusSO;
    [SerializeField] ActorStateMachine _actorStateMachine;

    public IReadOnlyReactiveProperty<int> CurrentHp => _actorHpModel.CurrentHp;
    public ActorStatusSO ActorStatusSO => _actorStatusSO;
}
