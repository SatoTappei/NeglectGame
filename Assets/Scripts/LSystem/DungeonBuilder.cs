using UnityEngine;

/// <summary>
/// �_���W���������𐧌䂷��R���|�[�l���g
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] DungeonPassBuilder _dungeonPassBuilder;
    [SerializeField] DungeonRoomBuilder _dungeonRoomBuilder;
    [SerializeField] DungeonWaypointBuilder _dungeonWaypointBuilder;

    /* 
     *  �_���W���������̃��[��
     *  �����̕���3�ȏ�̊
     *  �����̉��s��3�ȏ�̐�
     */

    /// <summary>���̃��\�b�h���O������ĂԂ��ƂŃ_���W���������������</summary>
    public void Build()
    {
        string result = _lSystem.Generate();

        _dungeonPassBuilder.BuildDungeonPass(result);
        _dungeonPassBuilder.FixPassVisual();
        var passMassDic = _dungeonPassBuilder.PassMassDic;

        _dungeonRoomBuilder.BuildDungeonRoom(passMassDic);
        var roomEntranceDic = _dungeonRoomBuilder.RoomEntranceDic;

        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);

        var passWaypointList = _dungeonPassBuilder.WaypointList;
        var roomWaypointList = _dungeonRoomBuilder.RoomEntranceDic.Keys;
        _dungeonWaypointBuilder.BuildPassWaypoint(passWaypointList);
        _dungeonWaypointBuilder.BuildRoomWaypoint(roomWaypointList);
    }
}