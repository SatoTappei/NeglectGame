/// <summary>
/// ステートマシン側が各ステートの遷移をするのに必要な処理を実装するインターフェース
/// 各ステートはこのインターフェースで実装される処理を組み合わせてステート内の処理を作っていく
/// </summary>
public interface IStateControl
{
    void PlayAnimation(string name);
    void PlayGoalPerformance();
    void PlayDeadPerformance();
    void MoveToWaypoint();
    void MoveToExit();
    void RunToInactiveLookInSight(SightableObject target);
    void RunTo(SightableObject target);
    void MoveCancel();
    void AffectAroundEffectableObject(string message);
    float GetAnimationClipLength(string name);
    bool IsTargetPosArrival();
    bool IsBelowHpThreshold();
    SightableObject GetInSightAvailableMovingTarget();
}
