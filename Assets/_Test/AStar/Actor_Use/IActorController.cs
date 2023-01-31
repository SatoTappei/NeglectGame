using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで使用する、各ステートの行動＆遷移条件を実装するインターフェース
/// </summary>
public interface IActorController : IMoveState, 
                                    IPlayAppearAnimState, 
                                    IPlayWanderAnimState, 
                                    IPanicState, 
                                    IDeadState
{
    public bool IsTransitionable();
}

public interface IMoveState
{
    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();
}

public interface IPlayAppearAnimState
{
    public void PlayAppearAnim();
}

public interface IPlayWanderAnimState
{
    public void PlayWanderAnim();
}

public interface IPanicState
{
    public bool IsTransitionToPanicState();
    public void PlayPanicAnim();
}

public interface IDeadState
{
    public bool IsTransitionToDeadState();
    public void PlayDeadAnim();
}