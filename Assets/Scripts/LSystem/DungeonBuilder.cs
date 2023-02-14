using UnityEngine;

/// <summary>
/// ダンジョン生成を制御するコンポーネント
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] DungeonPassBuilder _dungeonPassBuilder;
    [SerializeField] DungeonRoomBuilder _dungeonRoomBuilder;

    /*
     ダンジョン生成のルール
        部屋の幅は3以上の奇数
        部屋の奥行は3以上の数
     */

    /// <summary>このメソッドを外部から呼ぶことでダンジョンが生成される</summary>
    public void Build()
    {
        string result = _lSystem.Generate();

        _dungeonPassBuilder.BuildDungeonPass(result);
        var massDataAll = _dungeonPassBuilder.GetMassDataAll();

        _dungeonRoomBuilder.BuildDungeonRoom(massDataAll);
        var roomEntranceDic = _dungeonRoomBuilder.GetRoomEntranceDataAll();

        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);
    }
}
