/// <summary>
/// �ڕW��B�����ăS�[���ɓ��B�����Ƃ��̃X�e�[�g�̃N���X
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
