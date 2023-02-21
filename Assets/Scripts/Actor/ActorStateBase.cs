/// <summary>
/// ActorStateMachineで使用する各ステートの基底クラス
/// </summary>
public abstract class ActorStateBase
{
    protected enum Stage
    {
        Enter,
        Stay,
        Exit,
    }

    protected Stage _stage;
    protected ActorStateBase _nextState;
    protected ActorStateMachine _stateMachine;

    internal ActorStateBase(ActorStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _stage = Stage.Enter;
    }

    internal ActorStateBase Update()
    {
        if (_stage == Stage.Enter)
        {
            _stage = Stage.Stay;
            Enter();
        }
        else if (_stage == Stage.Stay)
        {
            Stay();
        }
        else if (_stage == Stage.Exit)
        {
            _stage = Stage.Enter;
            Exit();
            return _nextState;
        }

        return this;
    }

    protected virtual void Enter() { }
    protected virtual void Stay() { }
    protected virtual void Exit() { }

    protected void ChangeState(StateType stateType)
    {
        _nextState = _stateMachine.GetState(stateType);
        _stage = Stage.Exit;
    }
}
