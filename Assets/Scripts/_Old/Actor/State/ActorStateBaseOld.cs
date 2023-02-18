/// <summary>
/// キャラクターのステートマシンの各ステートの基底クラス
/// </summary>
internal abstract class ActorStateBaseOld
{
    protected enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    protected Stage _stage;
    protected ActorStateBaseOld _nextState;
    protected ActorStateMachineOld _stateMachine;

    internal ActorStateBaseOld(ActorStateMachineOld stateMachine)
    {
        _stateMachine = stateMachine;
        _stage = Stage.Enter;
    }

    internal ActorStateBaseOld Update()
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

    protected void ChangeState(StateIDOld stateID)
    {
        _nextState = _stateMachine.GetState(stateID);
        _stage = Stage.Exit;
    }
}