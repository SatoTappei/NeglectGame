using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの視界に映ったものを移動することのできる対象か判断するクラス
/// </summary>
public class ActorInSightFilter
{
    /// <summary>ダンジョンを生成する際にインスペクターで割り当てた部屋の数を初期容量として設定</summary>
    static readonly int UnAvailableMovingTargetListCap = 16;

    Dictionary<SightableType, List<Vector3>> _unAvailableMovingTargetDic;

    public ActorInSightFilter()
    {
        _unAvailableMovingTargetDic = new();
        foreach(SightableType type in Enum.GetValues(typeof(SightableType)))
        {
            _unAvailableMovingTargetDic.Add(type, new List<Vector3>(UnAvailableMovingTargetListCap));
        }
    }

    /// <returns>inSightObject もしくは null</returns>
    public SightableObject FilteringAvailableMoving(SightableObject inSightObject)
    {
        List<Vector3> list = _unAvailableMovingTargetDic[inSightObject.SightableType];
        if (list.Contains(inSightObject.transform.position))
        {
            return null;
        }
        else
        {
            return inSightObject;
        }
    }

    public void AddUnAvailableMovingTarget(SightableObject inSightObject)
    {
        List<Vector3> list = _unAvailableMovingTargetDic[inSightObject.SightableType];
        list.Add(inSightObject.transform.position);
    }

    // TODO:必要に応じて部屋の出入り口以外に対しても同様の処理が出来るように直す
    //      今のところ部屋の出入り口のみチェックを行えればよいので処理をラップしただけになっている
    //public bool IsAvailable(Vector3 pos)
    //{
    //    return !_unAvailabledRoomEntranceList.Contains(pos);
    //}

    //public void AddUnAvailableRoomEntrance(Vector3 pos)
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
}
