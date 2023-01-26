using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステートマシンの各ステートの基底クラス
/// </summary>
internal abstract class ActorStateBase
{
    protected IActorController _movable;

    protected enum Event
    {
        Enter,
        Stay,
        Exit,
    }

    public ActorStateBase(IActorController movable)
    {
        _movable = movable;
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

/// <summary>
/// ターゲットに向かって移動するステートのクラス
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    public ActorStateMove(IActorController movable) : base(movable) { }

    protected override void Enter()
    {
        _movable.MoveToTarget();
        base.Enter();
    }
}
