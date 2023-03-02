/// <summary>
/// 視界に入ったオブジェクトによって次のステートを選択するステートのクラス
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
            Debug.LogWarning("SightableTypeの値がおかしいです: " + inSightObject.SightableType);
        }
    }
}
