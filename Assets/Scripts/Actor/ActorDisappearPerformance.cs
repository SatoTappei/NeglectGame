using UnityEngine;

/// <summary>
/// キャラクターが消滅する際の演出のコンポーネント
/// </summary>
public class ActorDisappearPerformance : MonoBehaviour
{
    [Header("ゴール到達演出として出現するプレハブ")]
    [SerializeField] GameObject _goalPerformancePrefab;
    [Header("死亡演出として出現するプレハブ")]
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
