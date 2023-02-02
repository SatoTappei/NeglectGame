using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// キャラクターのステートマシンの各ステートの基底クラス
/// </summary>
internal abstract class ActorStateBase
{
    protected enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    protected IStateControl _stateControl;
    protected Stage _stage;
    protected ActorStateBase _nextState;
    protected ActorStateMachine _stateMachine;

    internal ActorStateBase(IStateControl _stateControl, ActorStateMachine stateMachine)
    {
        this._stateControl = _stateControl;
        _stateMachine = stateMachine;
        _stage = Stage.Enter;
    }

    internal ActorStateBase Update()
    {
        if (_stage == Stage.Enter)
        {
            Enter();
            _stage = Stage.Stay;
        }
        else if (_stage == Stage.Stay)
        {
            Stay();
        }
        else if (_stage == Stage.Exit)
        {
            Exit();
            _stage = Stage.Enter;
            return _nextState;
        }

        return this;
    }

    protected virtual void Enter() { }
    protected virtual void Stay() { }
    protected virtual void Exit() { }

    protected void ChangeState(StateID stateID)
    {
        _nextState = _stateMachine.GetState(stateID);
        _stage = Stage.Exit;
    }
}