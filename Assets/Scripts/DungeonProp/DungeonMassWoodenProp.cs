using UnityEngine;

/// <summary>
/// ダンジョンの通路に配置する木材小物のコンポーネント
/// </summary>
public class DungeonMassWoodenProp : MonoBehaviour
{
    static readonly float Prob = 0.3f;
    static readonly float PosXRange = 0.9f;
    static readonly float PosZRange = 1.0f;

    [Header("木材のプレハブ")]
    [SerializeField] Transform _woodenProp;

    void Awake()
    {
        if (Random.value <= Prob)
        {
            float posX = Random.Range(-PosXRange, PosXRange);
            float posZ = Random.Range(-PosZRange, PosZRange);

            _woodenProp.localPosition = new Vector3(posX, 0, posZ);

            float angle = Random.Range(0, 180.0f);
            _woodenProp.eulerAngles = new Vector3(0, angle, 0);
        }
        else
        {
            _woodenProp.localScale = Vector3.zero;
        }
    }
}
