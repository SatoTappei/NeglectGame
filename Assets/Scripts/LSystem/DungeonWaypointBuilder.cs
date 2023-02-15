using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���W������Waypoint�����o������R���|�[�l���g
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("�f�o�b�O�p:Waypoint�̉����Ɏg�p����v���n�u")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomEntranceWaypointPrefab;

    public void VisualizeWaypoint(IReadOnlyCollection<Vector3Int> pos)
    {
        foreach (Vector3Int p in pos)
        {
            Instantiate(_passWaypointPrefab, p, Quaternion.identity);
        }
    }
}
