using UnityEngine;

/// <summary>
/// ダンジョンの通路に配置するたるのコンポーネント
/// </summary>
public class DungeonMassBarrelProp : MonoBehaviour
{
    static readonly float Prob = 0.15f;

    [Header("たるのプレハブ")]
    [SerializeField] Transform _barrelProp;

    void Awake()
    {
        if (Random.value > Prob)
        {
            Destroy(_barrelProp.gameObject);
        }
    }
}
