using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで操作するために使う、キャラクターの行動を実装するインターフェース
/// </summary>
public interface IActorController
{
    public bool IsTransitionIdleState();

    public void MoveToTarget(bool isDash);
    public bool IsTransitionToWanderStateFromMoveState();
    public bool IsTransitionToAnimationStateFromMoveState();
    public void MoveCancel();

    public bool IsTransitionAnimationState();
    public void PlayAnim();

    public void PlayLookAround();
    public bool IsTransitionToMoveStateFromWanderStateAfterLookAroundDOtweenAnimation();

    public bool IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState();

    public void PlayDiscoverAnim();
    public bool IsTransitionToMoveStateFromDiscoverState();

    public void FromAnyStateDead();
}
