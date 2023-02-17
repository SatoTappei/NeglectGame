using UnityEngine;

public enum WaypointType
{
    Pass,
    Room,
    Exit,
}

/// <summary>
/// �_���W������Waypoint�Ƃ��ċ@�\������R���|�[�l���g
/// </summary>
public class WaypointTag : MonoBehaviour
{
    [Header("Waypoint�̎�ނ����ʂ��邽�߂̗񋓌^")]
    [SerializeField] WaypointType _type;

    public WaypointType Type => _type;
}
