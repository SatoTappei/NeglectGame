using UnityEngine;

/// <summary>
/// �A�N�^�[�̎��E�ɓ������ۂɃI�u�W�F�N�g�̎�ނ𔻒肷�邽�߂̗񋓌^
/// </summary>
enum SightableType
{
    Treasure,
    Enemy,
}

/// <summary>
/// �A�N�^�[�̎��E�ɉf��I�u�W�F�N�g�̃R���|�[�l���g
/// </summary>
public class SightableObject : MonoBehaviour
{
    [Header("�A�N�^�[���猩���I�u�W�F�N�g�̎��")]
    [SerializeField] SightableType _sightableType;

    GameObject _currentWitness;

    internal SightableType SightableType => _sightableType;

    internal void SetWitness(GameObject obj) => _currentWitness = obj;
    internal void ReleaseWitness() => _currentWitness = null;
    internal bool HasWitness() => _currentWitness != null;
}