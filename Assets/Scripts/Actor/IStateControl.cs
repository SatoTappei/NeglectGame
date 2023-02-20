/// <summary>
/// ステートマシン側が各ステートの遷移をするのに必要な処理を実装するインターフェース
/// 各ステートはこのインターフェースで実装される処理を組み合わせてステート内の処理を作っていく
/// </summary>
public interface IStateControl
{
    void PlayAnimation(string name);
    void MoveToWaypoint();
    void MoveToInSightObject();

    float GetAnimationClipLength(string name);
    /// <summary>
    /// 視界内のオブジェクト(InSightObject)に移動する際にも使うのでメソッド名を変更する
    /// </summary>
    /// <returns></returns>
    bool IsArrivalWaypoint();
    SightableObject GetInSightObject();
}
