using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで使用する、各ステートの行動＆遷移条件を実装するインターフェース
/// </summary>
public interface IActorController
{
    // 全てのステートは基本この遷移条件に従う
    public bool IsTransitionable();

    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();

    public void PlayWanderAnim();
    public void PlayAppearAnim();
    public void PlayPanicAnim();

    // 複数のステートから遷移する
    public bool IsTransitionToPanicState();
    public bool IsTransitionToDeadState();

    public void PlayDeadAnim();
}