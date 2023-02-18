/// <summary>
/// 周囲を見渡すアニメーションをするステートのクラス
/// </summary>
internal class ActorStateLookAround : ActorStateBaseOld
{
    internal ActorStateLookAround(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.LookAround, StateIDOld.Move);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() && 
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Dead))
        {
            ChangeState(StateIDOld.Dead);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateIDOld.Move))
        {
            ChangeState(StateIDOld.Move);
        }
    }
}