using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョン生成を制御するコンポーネント
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] DungeonPassBuilder _dungeonPassBuilder;
    [SerializeField] DungeonRoomBuilder _dungeonRoomBuilder;
    [SerializeField] DungeonWaypointBuilder _dungeonWaypointBuilder;

    /* 
     *  ダンジョン生成のルール
     *  部屋の幅は3以上の奇数
     *  部屋の奥行は3以上の数
     */

    /// <summary>このメソッドを外部から呼ぶことでダンジョンが生成される</summary>
    public void Build()
    {
        string result = _lSystem.Generate();

        _dungeonPassBuilder.BuildDungeonPass(result);
        _dungeonPassBuilder.FixPassVisual();
        var passMassDic = _dungeonPassBuilder.PassMassDic;

        _dungeonRoomBuilder.BuildDungeonRoom(passMassDic);
        var roomEntranceDic = _dungeonRoomBuilder.RoomEntranceDic;

        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);

        IReadOnlyCollection<Vector3Int> passWaypoints = _dungeonPassBuilder.Waypoints;
        IEnumerable<Vector3Int> roomWaypoints = _dungeonRoomBuilder.RoomEntranceDic.Keys;
        IReadOnlyCollection<Vector3Int> estimateExitpositions = _dungeonPassBuilder.EstimateExits;
        _dungeonWaypointBuilder.BuildPassWaypoint(passWaypoints);
        _dungeonWaypointBuilder.BuildRoomWaypoint(roomWaypoints);
        _dungeonWaypointBuilder.BuildExitWaypointRandom(estimateExitpositions);
    }
}
