/// <summary>
/// 攻撃のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateAttack : ActorStateBaseOld
{
    internal ActorStateAttack(ActorStateMachineOld stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateIDOld.Attack, StateIDOld.Attack);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() &&
            _stateMachine.StateControl.IsEqualNextState(StateIDOld.Dead))
        {
            ChangeState(StateIDOld.Dead);
        }
        else if(_stateMachine.StateControl.IsCompleted() &&
                _stateMachine.StateControl.IsEqualNextState(StateIDOld.Escape))
        {
            ChangeState(StateIDOld.Escape);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateIDOld.Attack))
        {
            ChangeState(StateIDOld.Attack);
        }
    }
}
