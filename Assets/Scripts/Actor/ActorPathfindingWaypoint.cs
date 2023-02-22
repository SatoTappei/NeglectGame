using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各キャラクターが経路探索に使用するWaypointを管理するクラス
/// </summary>
public class ActorPathfindingWaypoint
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;
    List<Vector3> _unAvailabledRoomEntranceList;

    public ActorPathfindingWaypoint(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic)
    {
        _waypointDic = waypointDic;
        _unAvailabledRoomEntranceList = new (waypointDic[WaypointType.Room].Count);
    }

    // TODO:同じウェイポイントを連続で獲得してしまうのを避ける
    public Vector3 Get(WaypointType type)
    {
        List<Vector3> list = _waypointDic[type]; 
        Vector3 waypoint = list[Random.Range(0, list.Count)];

        return waypoint;
    }

    // TODO:必要に応じて部屋の出入り口以外に対しても同様の処理が出来るように直す
    //      今のところ部屋の出入り口のみチェックを行えればよいので処理をラップしただけになっている
    public bool IsAvailable(Vector3 pos)
    {
        return !_unAvailabledRoomEntranceList.Contains(pos);
    }

    //void AddAvailableRoomEntrance(Vector3 pos)
    //{
    //    if (_unAvailabledRoomEntranceList.Contains(pos)) return;

    //    // 部屋の出入り口以外の座標が渡されてくる可能性があるので
    //    // その座標が部屋の出入り口のリストに含まれているかチェックする
    //    bool isRoomEntrance = _waypointDic[WaypointType.Room].Contains(pos);
    //    if (isRoomEntrance)
    //    {
    //        if (_unAvailabledRoomEntranceList.Count == _waypointDic[WaypointType.Room].Count - 1)
    //        {
    //            _unAvailabledRoomEntranceList.Clear();
    //        }

    //        _unAvailabledRoomEntranceList.Add(pos);

    //        return;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("部屋の出入り口以外の座標を部屋の入口として調べようとしています: " + pos);

    //        return;
    //    }
    //}

    // 問題点:部屋に入る=>何もない=>うろうろに遷移=>部屋を見つける でループしてしまう
    
    // 確認した部屋に入れないようにすると、各部屋を確認したタイミングで全て何もなかった場合
    //  各キャラクターのタスクが完了できなくなる

    // 各部屋を全部確認したらまた最初から全部確認する みたいなギミックが欲しい

    // 部屋の出入口は視覚でWaypointを検知しているのでこのクラスを挟んでいない。

    // 現状部屋のWaypointは視覚でしか認知されていない
    // 現状ダンジョンの中央からしか湧いてこない
    // 出口のWaypointからランダムに出てくるようにし、ダンジョンから帰る際はその出口に向かう
}
