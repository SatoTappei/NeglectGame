using UnityEngine;

/// <summary>
/// ダンジョンの通路に配置するろうそく小物のコンポーネント
/// </summary>
public class DungeonMassCandleProp : MonoBehaviour
{
    static readonly float Prob = 0.3f;

    [Header("ろうそくのプレハブ")]
    [SerializeField] Transform _candleProp;

    void Awake()
    {
        if (Random.value > Prob)
        {
            _candleProp.localScale = Vector3.zero;
        }
    }
}
