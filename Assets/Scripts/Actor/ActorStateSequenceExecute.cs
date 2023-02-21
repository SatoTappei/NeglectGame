using System.Threading;

/// <summary>
/// 各種Sequenceを実行するステートのクラス
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine)
    {

    }

    protected override void Enter()
    {
        // このステートに遷移してくる際には視界の機能は切ってあるので
        // Sequence実行時に違うSightableTypeのオブジェクトが渡されることはない

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();

        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            // Sequenceの実行
            Debug.Log("宝発見Sequenceを実行");
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            //  敵を発見した
            //      n%の確率で結果が勝ち/負けのステートに遷移
            // Sequenceの実行

            Debug.Log("敵発見Sequenceを実行");
        }
        else
        {
            // 何も視界に無いのでExploreに戻る
            Debug.Log("探索に戻る");
        }
    }
}
