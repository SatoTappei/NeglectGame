using UnityEngine;

public class PathfindingTestControl : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] GameObject _actorPrefab;

    void Start()
    {
        _pathfindingGrid.GenerateGrid();
        _waypointManager.RegisterWaypoint();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_actorPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
