using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���W������Waypoint�𐶐�����R���|�[�l���g
/// ��������Waypoint�̓q�G�����L�[��̃I�u�W�F�N�g�̐e�q�\����ʂ��ĎQ�Ƃ����
/// </summary>
public class DungeonWaypointBuilder : MonoBehaviour
{
    [Header("�f�o�b�O�p:Waypoint�̉����Ɏg�p����v���n�u")]
    [SerializeField] GameObject _passWaypointVisualizer;
    [SerializeField] GameObject _roomWaypointVisualizer;
    [Header("�eWaypoint�Ƃ��Đ�������v���n�u")]
    [SerializeField] GameObject _passWaypointPrefab;
    [SerializeField] GameObject _roomWaypointPrefab;
    [SerializeField] GameObject _exitWaypointPrefab;
    [Header("��������Waypoint�̐e")]
    [SerializeField] Transform _waypointParent;

    internal void BuildPassWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _passWaypointPrefab);
        list.ForEach(go => go.transform.parent = _waypointParent);
    }

    internal void BuildRoomWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _roomWaypointPrefab);
        list.ForEach(go => go.transform.parent = _waypointParent);
    }

    internal void BuildExitWaypoint(IEnumerable<Vector3Int> positions)
    {
        List<GameObject> list = BuildWaypoint(positions, _exitWaypointPrefab);
        list.ForEach(go => go.transform.parent = _waypointParent);
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