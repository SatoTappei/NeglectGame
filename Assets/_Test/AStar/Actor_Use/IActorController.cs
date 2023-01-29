using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで操作するために使う、キャラクターの行動を実装するインターフェース
/// </summary>
public interface IActorController
{
    public void MoveToTarget();
    public void RunToTarget();
    public bool IsTransitionable();

    public bool IsTransitionToPanicState();
    public void CancelMoveToTarget();

    public void PlayAppearAnim();

    public void PlayWanderAnim();

    public bool IsTransitionToDeadState();

    public void PlayPanicAnim();

    public void PlayDeadAnim();
}
