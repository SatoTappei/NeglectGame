using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 経路探索に使用するWaypointを管理するコンポーネント
/// </summary>
public class WaypointManager : MonoBehaviour
{
    static readonly int DicCapacity = Enum.GetValues(typeof(WaypointType)).Length;

    [Header("Waypointが子として登録されているオブジェクト")]
    [SerializeField] Transform _wayPointParent;

    Dictionary<WaypointType, List<Vector3>> _waypointDic = new(DicCapacity);

    /* 
     * ★次の最優先タスク
     *  Waypointの生成、親への登録、参照する側がWaypointの親を取得までは出来た。 
     *  生成されたWaypointを取得してデータとして持つようにする
     */

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegisterWaypoint();
        }
    }

    void RegisterWaypoint()
    {
        foreach(Transform child in _wayPointParent)
        {
            if(child.TryGetComponent(out DungeonWaypoint component))
            {
                WaypointType type = component.Type;
                Vector3 pos = child.position;

                if (!_waypointDic.ContainsKey(type))
                {
                    _waypointDic.Add(type, new List<Vector3>());
                }

                _waypointDic[type].Add(pos);
            }
            else
            {
                Debug.LogError("Waypointとして使用できません: " + child.gameObject.name);
            }
        }
    }

    // TODO:前回と同じ地点をゴールとして選んでしまうと移動しなくなる不具合があるのでどうにかする
    //int _prev = -1;

    //Vector3 ITargetSelectable.GetExitPos()
    //{
    //    throw new System.NotImplementedException();
    //}

    //Vector3 ITargetSelectable.GetNextWaypointPos()
    //{
    //    int r;
    //    while (true)
    //    {
    //        r = Random.Range(0, _targetArr.Length);
    //        if (r != _prev)
    //        {
    //            _prev = r;
    //            break;
    //        }
    //    }

    //    return _targetArr[r].position;
    //}

    //Vector3 ITargetSelectable.GetExitPos() => _exit.position;
}

// Waypointの生成はダンジョン生成側が行う
// 通路、部屋の入口、階段があるので判別する何かが必要、後の追加も考える
// この座標のリストは○○のWaypointというやり方はいかがなものか
// 増えてきたら管理が難しくなる？
// 

// こちらは生成されたWaypointを読み取る
// DungeonWaypointVisualizer => Presetnerに生成
// WaypointTarget => Presetnerから読み取り
// 時間的結合が存在する Waypoint生成 => Targetで読み込み
// だがこれはダンジョン生成=>パスファインドのグリッド生成の順で行えばおｋなので問題なし