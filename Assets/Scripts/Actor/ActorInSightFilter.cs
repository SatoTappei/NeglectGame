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

    public SightableObject SelectMovingTarget(IEnumerable<SightableObject> inSightObjects)
    {
        foreach (SightableObject inSightObject in inSightObjects)
        {
            if (inSightObject.SightableType == SightableType.RoomEntrance)
            {
                // 部屋の入口だったら1度しか視界に捉えないようにする
                List<Vector3> list = _unAvailableMovingTargetDic[SightableType.RoomEntrance];
                if (!list.Contains(inSightObject.transform.position))
                {
                    return inSightObject;
                }
            }
            else
            {
                return inSightObject;
            }
        }

        return null;
    }

    public void AddUnAvailableMovingTarget(SightableObject inSightObject)
    {
        List<Vector3> list = _unAvailableMovingTargetDic[inSightObject.SightableType];
        list.Add(inSightObject.transform.position);
    }
}
