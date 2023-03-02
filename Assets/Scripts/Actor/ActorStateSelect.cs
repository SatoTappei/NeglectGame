/// <summary>
/// ���E�ɓ������I�u�W�F�N�g�ɂ���Ď��̃X�e�[�g��I������X�e�[�g�̃N���X
/// </summary>
public class ActorStateSelect : ActorStateBase
{
    public ActorStateSelect(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    protected override void Enter()
    {
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        if (inSightObject.SightableType == SightableType.RoomEntrance)
        {
            ChangeState(StateType.EnterTheRoom);
        }
        else if (inSightObject.SightableType == SightableType.Treasure ||
                 inSightObject.SightableType == SightableType.Enemy)
        {
            ChangeState(StateType.SequenceExecute);
        }
        else
        {
            Debug.LogWarning("SightableType�̒l�����������ł�: " + inSightObject.SightableType);
        }
    }
}
