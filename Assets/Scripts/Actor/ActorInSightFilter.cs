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
}
