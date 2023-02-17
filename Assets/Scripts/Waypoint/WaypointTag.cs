using UnityEngine;

public enum WaypointType
{
    Pass,
    Room,
    Exit,
}

/// <summary>
/// ダンジョンのWaypointとして機能させるコンポーネント
/// </summary>
public class WaypointTag : MonoBehaviour
{
    [Header("Waypointの種類を識別するための列挙型")]
    [SerializeField] WaypointType _type;

    public WaypointType Type => _type;
}
