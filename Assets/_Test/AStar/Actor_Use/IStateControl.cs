using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで使用する、各ステートの行動＆遷移条件を実装するインターフェース
/// </summary>
public interface IStateControl
{
    // 全てのステートは基本この遷移条件に従う
    public bool IsTransitionable();

    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();

    public void PlayLookAroundAnim();
    public void PlayAppearAnim();
    public void PlayPanicAnim();
    public void PlayJoyAnim();
    public void PlayAttackAnim();

    // 複数のステートから遷移する
    public bool IsTransitionToPanicState();
    public bool IsTransitionToDeadState();

    // ダッシュが終わったら呼ばれる、なんかフラグのオンオフとかする
    public void RunEndable();

    public void PlayDeadAnim();
}