using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 各種Sequenceを実行するステートのクラス
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    static readonly float BattleWinPercent = 0.5f;

    public ActorStateSequenceExecute(ActorStateMachine stateMachine, StateType type)
        : base(stateMachine, type) { }

    CancellationTokenSource _cts;

    protected override void Enter()
    {
        _cts = new CancellationTokenSource();

        if (_stateMachine.StateControl.IsBelowHpThreshold())
        {
            ExecuteSequence(SequenceType.Exit, StateType.Goal);
            return;
        }

        // このステートに遷移してくる際には視界の機能は切ってあるので
        // Sequence実行時に違うSightableTypeのオブジェクトが渡されることはない
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            ExecuteSequence(SequenceType.Treasure, StateType.Goal);
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            if(Random.value < BattleWinPercent)
            {
                ExecuteSequence(SequenceType.BattleWin, StateType.Goal);
            }
            else
            {
                ExecuteSequence(SequenceType.BattleLose, StateType.Dead);
            }
        }
        else
        {
            Debug.LogWarning("未定義の列挙型です。");
        }
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
        // このステートから遷移する = Sequence中になんかしらのイベントが発生してSequence中断
        _cts?.Cancel();
    }

    public override void OnStateMachinePause()
    {
        _cts?.Cancel();
    }

    void ExecuteSequence(SequenceType sequenceType, StateType transitionStateType)
    {
        ActorStateSequence sequence = _stateMachine.GetSequence(sequenceType);
        sequence.ExecuteAsync(_cts, () => TryChangeState(transitionStateType)).Forget();
    }
}
