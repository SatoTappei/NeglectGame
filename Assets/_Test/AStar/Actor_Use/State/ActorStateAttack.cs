/// <summary>
/// 攻撃のアニメーションを行うステートのクラス
/// </summary>
internal class ActorStateAttack : ActorStateBase
{
    internal ActorStateAttack(IStateControl stateControl, ActorStateMachine stateMachine)
        : base(stateControl, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAnim(StateID.Attack, StateID.Non);
    }

    protected override void Stay()
    {
        if (_stateControl.IsDead() && _stateControl.IsEqualNextState(StateID.Dead))
        {
            ChangeState(StateID.Dead);
        }
        else if (_stateControl.IsTransitionable() && _stateControl.IsEqualNextState(StateID.Escape))
        {
            ChangeState(StateID.Escape);
        }
    }
}
