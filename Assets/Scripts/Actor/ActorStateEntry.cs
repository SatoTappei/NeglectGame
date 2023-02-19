using DG.Tweening;

/// <summary>
/// ActorStateMachineで使用する登場時の演出を行うステートのクラス
/// </summary>
public class ActorStateEntry : ActorStateBase
{
    public ActorStateEntry(ActorStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    protected override void Enter()
    {
        _stateMachine.StateControl.PlayAnimation("Entry");

        // アニメーションの長さ分だけDelayすることで、アニメーションに合わせた遷移に見せる
        float delayTime = _stateMachine.StateControl.GetAnimationClipLength("Entry");
        DOVirtual.DelayedCall(delayTime, () =>
        {
            ChangeState(StateType.Explore);
        }).SetLink(_stateMachine.gameObject);
    }
}
