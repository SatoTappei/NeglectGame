using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各Waypointを辞書型で管理するプロパティを実装させるインターフェース
/// キャラクター側でこのインターフェースを介して取得する
/// </summary>
public interface IWaypointManage
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> WaypointDic { get; }
}
