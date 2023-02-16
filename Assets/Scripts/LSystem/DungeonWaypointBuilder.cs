using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���W������Waypoint�����o������R���|�[�l���g
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("�f�o�b�O�p:Waypoint�̉����Ɏg�p����v���n�u")]
    [SerializeField] GameObject _passWaypointVisualizer;
    [SerializeField] GameObject _roomWaypointVisualizer;

    /// <summary>
    /// �o�H�T���̃R���|�[�l���g���œǂݎ�点��ׂ�
    /// ���̃I�u�W�F�N�g�̎q�Ƃ���Waypoint�𐶐�����
    /// </summary>
    [Header("��������Waypoint�̐e")]
    [SerializeField] Transform _dungeonWaypointParent;
    [Header("�eWaypoint�Ƃ��Đ�������v���n�u")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomWaypointPrefab;
    [SerializeField] GameObject _exitWaypointPrefab;

    internal void BuildPassWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _passWaypointPrefab);
        list.ForEach(go => go.transform.parent = _dungeonWaypointParent);
    }

    internal void BuildRoomWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _roomWaypointPrefab);
        list.ForEach(go => go.transform.parent = _dungeonWaypointParent);
    }

    internal void BuildExitWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _exitWaypointPrefab);
        list.ForEach(go => go.transform.parent = _dungeonWaypointParent);
    }

    internal void VisualizePassWaypoint(IEnumerable<Vector3Int> positions)
    {
        BuildWaypoint(positions, _passWaypointVisualizer);
    }

    internal void VisualizeRoomWaypoint(IEnumerable<Vector3Int> positions)
    {
        BuildWaypoint(positions, _roomWaypointVisualizer);
    }

    List<GameObject> BuildWaypoint(IEnumerable<Vector3Int> positions, GameObject prefab)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (Vector3Int pos in positions)
        {
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            list.Add(go);
        }

        return list;
    }
}
