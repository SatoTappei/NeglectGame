using UnityEngine;

/// <summary>
/// �_���W�����̒ʘH�ɔz�u����낤���������̃R���|�[�l���g
/// </summary>
public class DungeonMassCandleProp : MonoBehaviour
{
    static readonly float Prob = 0.3f;

    [Header("�낤�����̃v���n�u")]
    [SerializeField] Transform _candleProp;

    void Awake()
    {
        if (Random.value > Prob)
        {
            _candleProp.localScale = Vector3.zero;
        }
    }
}
