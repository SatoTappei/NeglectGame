using UnityEngine;

/// <summary>
/// �_���W�����̒ʘH�ɔz�u����؍ޏ����̃R���|�[�l���g
/// </summary>
public class DungeonMassWoodenProp : MonoBehaviour
{
    static readonly float Prob = 0.3f;
    static readonly float PosXRange = 0.9f;
    static readonly float PosZRange = 1.0f;

    [Header("�؍ނ̃v���n�u")]
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
