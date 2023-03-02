/// <summary>
/// 目標を達成してゴールに到達したときのステートのクラス
/// </summary>
public class ActorStateGoal : ActorStateBase
{
    public ActorStateGoal(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayGoalPerformance();
    }
}
