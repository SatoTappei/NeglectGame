/// <summary>
/// 視界に入ったオブジェクトによって次のステートを選択するステートのクラス
/// </summary>
public class ActorStateSelect : ActorStateBase
{
    public ActorStateSelect(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        
        //if (inSightObject == null)
        //{
        //    ChangeState(StateType.Explore);
        //    Debug.LogWarning("InSightSelectステート内でinSightObjectがnullなのでExploreステートに遷移します");
        //    return;
        //}

        // 次フレームでステートの処理が呼ばれるまでにInSightObjectの値が変化しないように
        // 視界の機能を切っておく
        //_stateMachine.StateControl.ToggleSight(isActive: false);

        if (inSightObject.SightableType == SightableType.RoomEntrance)
        {
            ChangeState(StateType.EnterTheRoom);
        }
        else if (inSightObject.SightableType == SightableType.Treasure ||
                 inSightObject.SightableType == SightableType.Enemy)
        {
            Debug.Log("宝/敵を発見" + _stateMachine.gameObject.GetHashCode());
            //_stateMachine.transform.localScale = UnityEngine.Vector3.one * 2;
            //ChangeState(StateType.SequenceExecute);
        }
        else
        {
            Debug.LogWarning("SightableTypeの値がおかしいです: " + inSightObject.SightableType);
        }
    }
}
