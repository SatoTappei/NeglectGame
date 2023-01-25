using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートのベース
/// </summary>
internal abstract class ActorStateBase
{
    protected IMovable _movable;
    protected PathfindingTargetDecider _pathfindingTargetDecider;

    protected enum Event
    {
        Enter,
        Stay,
        Exit,
    }

    public ActorStateBase(IMovable movable, PathfindingTargetDecider targetDecider)
    {
        _movable = movable;
        _pathfindingTargetDecider = targetDecider;
    }

    protected Event _event;

    protected virtual void Enter() => _event = Event.Stay;
    protected virtual void Stay() => _event = Event.Stay;
    protected virtual void Exit() => _event = Event.Exit;

    public void Update()
    {
        if      (_event == Event.Enter) Enter();
        else if (_event == Event.Stay)  Stay();
        else if (_event == Event.Exit)  Exit();
    }
}

internal class ActorStateMove : ActorStateBase
{
    public ActorStateMove(IMovable movable, PathfindingTargetDecider targetDecider)
        : base(movable, targetDecider)
    {
        // 処理無し
    }

    protected override void Enter()
    {
        _movable.MoveStart(_pathfindingTargetDecider.GetPathfindingTarget());
        base.Enter();
    }
}
