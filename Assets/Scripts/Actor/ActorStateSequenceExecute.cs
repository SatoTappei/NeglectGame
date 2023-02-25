using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 各種Sequenceを実行するステートのクラス
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    static readonly float BattleWinPercent = 0.7f;

    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine) { }

    CancellationTokenSource _cts;

    protected override void Enter()
    {
        // このステートに遷移してくる際には視界の機能は切ってあるので
        // Sequence実行時に違うSightableTypeのオブジェクトが渡されることはない
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();

        _cts = new CancellationTokenSource();

        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.Treasure);
            sequence.ExecuteAsync(_cts, () => 
            {
                TryChangeState(StateType.Goal);
            }).Forget();
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            if(Random.value < BattleWinPercent)
            {
                ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.BattleWin);
                sequence.ExecuteAsync(_cts, () =>
                {
                    TryChangeState(StateType.Goal);
                }).Forget();
            }
            else
            {
                ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.BattleLose);
                sequence.ExecuteAsync(_cts, () =>
                {
                    TryChangeState(StateType.Dead);
                }).Forget();
            }
        }
        else
        {
            // 何も視界に無いのでExploreに戻る
            Debug.LogWarning("未定義の列挙型です。");
        }
    }

    protected override void Stay()
    {
        // 体力が0以下になったら死ぬ
    }

    protected override void Exit()
    {
        // このステートから遷移する = Sequence中になんかしらのイベントが発生してSequence中断
        _cts?.Cancel();
    }
}
