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
        transform.localScale = Vector3.zero;
        Instantiate(_goalPerformancePrefab, transform.position, Quaternion.identity);
    }

    public void PlayDeadPerformance()
    {
        transform.localScale = Vector3.zero;
        Instantiate(_deadPerformancePrefab, transform.position, Quaternion.identity);
    }
}
