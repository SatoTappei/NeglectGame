/// <summary>
/// 攻撃のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateAttack : ActorStateBase
{
    internal ActorStateAttack(ActorStateMachine stateMachine)
        : base(stateMachine) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnim(StateID.Attack, StateID.Non);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsDead() &&
            _stateMachine.StateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateMachine.StateControl.IsTransitionable() &&
                 _stateMachine.StateControl.IsEqualNextState(StateID.Escape))
        {
            ChangeState(StateID.Escape);
        }
    }
}
