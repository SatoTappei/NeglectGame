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

    internal ActorStateBase(ActorStateMachine stateMachine, StateType type)
    {
        _stateMachine = stateMachine;
        _stage = Stage.Enter;
        Type = type;
    }

    public StateType Type { get; }

    /// <summary>
    /// ChangeState()が呼ばれたフレームではなく、次のフレームでExit()を呼び出し
    /// その次のフレームから次のステートに切り替わる
    /// </summary>
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

    /// <summary>先に遷移処理が呼ばれていた場合はこの遷移処理をキャンセルする</summary>
    protected bool TryChangeState(StateType type)
    {
        if (_stage == Stage.Stay)
        {
            ChangeState(type);
            return true;
        }
        else
        {
            Debug.LogWarning("既に別のステートに遷移する処理が呼ばれています: " + type);
            return false;
        }
    }
}
