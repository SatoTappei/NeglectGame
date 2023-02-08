/// <summary>
/// ステートマシンで使用する、各ステートの行動＆遷移条件を実装するインターフェース
/// </summary>
internal interface IStateControl
{
    // Controller側にインターフェースが実装されているので
    // このメソッド群はControllerの機能を使わせてあげるという実装になる

    // TODO:メソッド名がよくない
    void PlayAnim(StateID current, StateID next);
    void CancelAnim(string name);

    void MoveToTarget();
    void RunToTarget();
    void MoveToExit();
    void CancelMoveToTarget();

    // アニメーションが終わったタイミングで次のステートがせっとされるからいらないかもしれない
    bool IsTransitionable();
    bool IsEqualNextState(StateID state);
    bool IsDead();

    // 攻撃ステートでしか使われていない、戦っている敵が死んだか判定する処理
    bool IsTargetLost();

    //// 全てのステートは基本この遷移条件に従う
    //public bool IsTransitionable();

    bool IsSightTarget();
}