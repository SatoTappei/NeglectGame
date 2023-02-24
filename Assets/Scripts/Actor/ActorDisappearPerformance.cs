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
        Instantiate(_goalPerformancePrefab);
        Destroy(gameObject);
    }

    public void PlayDeadPerformance()
    {
        Instantiate(_deadPerformancePrefab);
        Destroy(gameObject);
    }
}
