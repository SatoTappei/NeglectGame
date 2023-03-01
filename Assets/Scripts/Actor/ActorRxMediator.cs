using UniRx;
using UnityEngine;

/// <summary>
/// UniRx�Ńf�[�^����舵���ۂɕK�v�ȎQ�ƂƂ̒�����ɂȂ�R���|�[�l���g
/// ���g�p
/// </summary>
public class ActorRxMediator : MonoBehaviour
{
    [SerializeField] ActorHpControl _actorHpModel;
    [SerializeField] ActorStatusSO _actorStatusSO;
    [SerializeField] ActorStateMachine _actorStateMachine;

    public IReadOnlyReactiveProperty<int> CurrentHp => _actorHpModel.CurrentHp;
    public ActorStatusSO ActorStatusSO => _actorStatusSO;
}
