using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    void Start()
    {
        // ダンジョン生成時にアニメーションさせるのでCapacityを増やして警告を消す
        // 処理負荷が問題になった場合はアニメーションをやめること
        DOTween.SetTweensCapacity(500, 50);

        string result = _lSystem.Generate();

        _dungeonPassBuilder.BuildDungeonPass(result);
        var massDataAll = _dungeonPassBuilder.GetMassDataAll();

        _dungeonRoomBuilder.BuildDungeonRoom(massDataAll);
        var roomEntranceDic = _dungeonRoomBuilder.GetRoomEntranceDataAll();

        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);
    }
}
