/// <summary>
/// ActorStateMachine�Ŏg�p����e�X�e�[�g�̊��N���X
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

    protected void ChangeState(StateType stateType)
    {
        _nextState = _stateMachine.GetState(stateType);
        _stage = Stage.Exit;
    }
}
