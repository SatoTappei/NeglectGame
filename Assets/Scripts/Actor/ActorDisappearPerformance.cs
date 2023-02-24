using UnityEngine;

/// <summary>
/// �L�����N�^�[�����ł���ۂ̉��o�̃R���|�[�l���g
/// </summary>
public class ActorDisappearPerformance : MonoBehaviour
{
    [Header("�S�[�����B���o�Ƃ��ďo������v���n�u")]
    [SerializeField] GameObject _goalPerformancePrefab;
    [Header("���S���o�Ƃ��ďo������v���n�u")]
    [SerializeField] GameObject _deadPerformancePrefab;

    public void PlayGoalPerformance()
    {
        Instantiate(_goalPerformancePrefab);
        Destroy(gameObject);
    }

    public void PlayDeadPerformance()
    {
        Instantiate(_deadPerformancePrefab);
        Destroy(gameObject);
    }
}
