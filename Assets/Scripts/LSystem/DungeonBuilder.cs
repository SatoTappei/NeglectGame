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
        var massDataAll = _dungeonPassBuilder.PassMassDic;

        _dungeonRoomBuilder.BuildDungeonRoom(massDataAll);
        var roomEntranceDic = _dungeonRoomBuilder.GetRoomEntranceDataAll();

        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);

        // 十字路,T字路,部屋の入口にWayPointを敷く
        var waypointPosList = _dungeonPassBuilder.GetWaypointAll();
        _dungeonWaypointBuilder.VisualizeWaypoint(waypointPosList);
    }
}
