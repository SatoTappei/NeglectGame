using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートのベース
/// </summary>
internal abstract class ActorStateBase
{
    protected enum Event
    {
        Enter,
        Stay,
        Exit,
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

}
