using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで使用する、各ステートの行動＆遷移条件を実装するインターフェース
/// </summary>
public interface IActorController
{
    public bool IsTransitionable();

    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();

    public bool IsTransitionToPanicState();
    public void PlayPanicAnim();

    public void PlayAppearAnim();

    public void PlayWanderAnim();

    public bool IsTransitionToDeadState();
    public void PlayDeadAnim();
}
