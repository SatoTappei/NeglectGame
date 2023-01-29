using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで操作するために使う、キャラクターの行動を実装するインターフェース
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
