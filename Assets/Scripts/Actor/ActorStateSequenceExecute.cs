using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// 各種Sequenceを実行するステートのクラス
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        // このステートに遷移してくる際には視界の機能は切ってあるので
        // Sequence実行時に違うSightableTypeのオブジェクトが渡されることはない

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();

        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.Treasure);
            sequence.Execute(_stateMachine.gameObject.GetCancellationTokenOnDestroy());
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            //  敵を発見した
            //      n%の確率で結果が勝ち/負けのステートに遷移
            // Sequenceの実行

            ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.BattleWin);
            sequence.Execute(_stateMachine.gameObject.GetCancellationTokenOnDestroy());
        }
        else
        {
            // 何も視界に無いのでExploreに戻る
            Debug.Log("探索に戻る");
        }
    }
}
