/// <summary>
/// ステートマシンで使用する、各ステートの行動＆遷移条件を実装するインターフェース
/// </summary>
internal interface IStateControlOld
{
    // TODO:メソッド名がよくない
    //      遷移先のステート名とそのステートから通常遷移するステート名が引数
    void PlayAnim(StateType current, StateType next);

    void MoveToRandomWaypoint();
    void RunToTarget();
    void MoveToExit();
    void CancelMoving();

    bool IsTransitionable();
    bool IsEqualNextState(StateType state);
    bool IsDead();
    bool IsSightTarget();
    bool IsCompleted();

    // プレイヤーの周囲のオブジェクトの操作をさせる
    void EffectAroundEffectableObject();
}