using UnityEngine;

/// <summary>
/// 経路探索で向かうターゲットの選択機能を実装させるインターフェース
/// </summary>
internal interface ITargetSelectable
{
    Vector3 GetNextWaypointPos();
    Vector3 GetExitPos();
}
