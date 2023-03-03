using UniRx;
using UnityEngine;

/// <summary>
/// ���������L�����N�^�[���Ď�����R���|�[�l���g
/// </summary>
public class ActorMonitor : MonoBehaviour
{
    ReactiveProperty<int> _defeatedCount = new();

    public IReadOnlyReactiveProperty<int> DefeatedCount => _defeatedCount;

    public void DetectGoalOrDeadState(StateType state)
    {
        if (state == StateType.Dead)
        {
            _defeatedCount.Value++;
        }
    }
}
