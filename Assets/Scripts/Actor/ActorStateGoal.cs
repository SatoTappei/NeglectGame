/// <summary>
/// �ڕW��B�����ăS�[���ɓ��B�����Ƃ��̃X�e�[�g�̃N���X
/// </summary>
public class ActorStateGoal : ActorStateBase
{
    public ActorStateGoal(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayGoalEffect();
    }
}
