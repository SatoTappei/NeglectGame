/// <summary>
/// 目標を達成してゴールに到達したときのステートのクラス
/// </summary>
public class ActorStateGoal : ActorStateBase
{
    public ActorStateGoal(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayGoalEffect();
    }
}
