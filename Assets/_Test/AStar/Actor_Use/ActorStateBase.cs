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

    protected IActorController _actorController;
    protected Stage _stage;
    protected ActorStateBase _nextState;
    protected ActorStateMachine _stateMachine;

    internal ActorStateBase(IActorController actorController, ActorStateMachine stateMachine)
    {
        _actorController = actorController;
        _stateMachine = stateMachine;
        _stage = Stage.Enter;
    }

    internal ActorStateBase Update()
    {
        if      (_stage == Stage.Enter) Enter();
        else if (_stage == Stage.Stay)  Stay();
        else if (_stage == Stage.Exit)
        {
            Exit();
            _stage = Stage.Enter;
            return _nextState;
        }

        return this;
    }

    // 各ステートでオーバーライドした際、メソッドの"最後"で base. を呼ぶこと
    protected virtual void Enter() => _stage = Stage.Stay;
    protected virtual void Stay() => _stage = Stage.Stay;
    protected virtual void Exit() => _stage = Stage.Exit;

    protected void ChangeState(StateID stateID)
    {
        _nextState = _stateMachine.GetState(stateID);
        _stage = Stage.Exit;
    }
}