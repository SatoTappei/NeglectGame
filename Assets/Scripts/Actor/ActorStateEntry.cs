using DG.Tweening;

/// <summary>
/// 登場時の演出を行うステートのクラス
/// </summary>
public class ActorStateEntry : ActorStateBase
{
    Tween _tween;

    public ActorStateEntry(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnimation("Entry");

        // アニメーションの長さ分だけDelayすることで、アニメーションに合わせた遷移に見せる
        float delayTime = _stateMachine.StateControl.GetAnimationClipLength("Entry");
        _tween = DOVirtual.DelayedCall(delayTime, () =>
        {
            ChangeState(StateType.Explore);
        }).SetLink(_stateMachine.gameObject);
    }

    protected override void Stay()
    {
        if (_stateMachine.StateControl.IsHpEqualZero())
        {
            TryChangeState(StateType.Dead);
            return;
        }
    }

    protected override void Exit()
    {
        _tween?.Kill();
    }

    public override void OnStateMachinePause()
    {
        _tween?.Kill();
    }
}
