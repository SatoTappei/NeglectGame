using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎��E�ɓ������ۂɃI�u�W�F�N�g�̎�ނ𔻒肷�邽�߂̗񋓌^
/// </summary>
enum SightableType
{
    RoomEntrance,
    Treasure,
    Enemy,
}

/// <summary>
/// �L�����N�^�[�̎��E�ɉf��I�u�W�F�N�g�̃R���|�[�l���g
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class SightableObject : MonoBehaviour
{
    [Header("�I�u�W�F�N�g�̔��ʂɎg�����")]
    [SerializeField] SightableType _sightableType;

    internal SightableType SightableType => _sightableType;

    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Sightable");

        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    /// <summary>�L�����N�^�[�̎��E�ɉf��z��ł̉^�p�Ȃ̂�Actor�^�������Ɏ��</summary>
    public virtual bool IsAvailable(Actor _) => true;
}