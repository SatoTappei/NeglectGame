using UnityEngine;

/// <summary>
/// ステートマシン側が各ステートの遷移をするのに必要な処理を実装するインターフェース
/// 各ステートはこのインターフェースで実装される処理を組み合わせてステート内の処理を作っていく
/// </summary>
public interface IStateControl
{
    void PlayAnimation(string name);
    void MoveToWaypoint();
    void MoveTo(Vector3 targetPos);
    void ToggleSight(bool isActive);
    float GetAnimationClipLength(string name);
    bool IsArrivalTargetPos();
    SightableObject GetInSightObject();
}
