/// <summary>
/// 視界に入ったオブジェクトによってステートを選択するステートのクラス
/// </summary>
public class ActorStateInSightSelect : ActorStateBase
{
    public ActorStateInSightSelect(ActorStateMachine stateMachine) : base(stateMachine)
    {

    }

    protected override void Enter()
    {
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();
        if (inSightObject == null)
        {
            ChangeState(StateType.Explore);
            Debug.LogWarning("InSightSelectステート内でinSightObjectがnullなのでExploreステートに遷移します");
        }
        else
        {
            // 次フレームでステートの処理が呼ばれるまでにInSightObjectの値が変化しないように
            // 視界の機能を切っておく
            _stateMachine.StateControl.ToggleSight(isActive: false);

            if (inSightObject.SightableType == SightableType.RoomEntrance)
            {
                ChangeState(StateType.MoveToRoom);
            }
            else
            {
                ChangeState(StateType.SequenceExecute);
            }
        }
    }
}
