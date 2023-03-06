using UnityEngine;

/// <summary>
/// �_���W�����̒ʘH�ɔz�u���邽��̃R���|�[�l���g
/// </summary>
public class DungeonMassBarrelProp : MonoBehaviour
{
    static readonly float Prob = 0.15f;

    [Header("����̃v���n�u")]
    [SerializeField] Transform _barrelProp;

    void Awake()
    {
        if (Random.value > Prob)
        {
            Destroy(_barrelProp.gameObject);
        }
    }
}
