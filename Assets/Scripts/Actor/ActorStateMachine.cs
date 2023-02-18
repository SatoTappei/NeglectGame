using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public enum StateType
{
    Entry,
    Explore,
    Dead,
}

/// <summary>
/// キャラクターの行動を制御するステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    CancellationTokenSource _cts = new CancellationTokenSource();

    void Start()
    {
        // 基準ステート
        //  ランダムなWaypointに向けて移動する

        // やる気が一定以下のSequence
        //  脱出(位置に到着)

        // 宝箱を発見したときのSequence
        //  見つけたアニメーション(アニメーション終了)
        //  対象に向かってダッシュ(位置に到着)
        //  獲得のアニメーション(アニメーション終了)

        // 敵を発見したときのSequence(結果が勝ち)
        //  見つけたアニメーション(アニメーション終了)
        //  対象に向かってダッシュ(位置に到着)
        //  戦闘する(勝ち)(アニメーション終了)

        // 敵を発見したときのSequence(結果が負け)
        //  見つけたアニメーション(アニメーション終了)
        //  対象に向かってダッシュ(位置に到着)
        //  戦闘する(負け)(アニメーション終了)
        //  死亡ステートに遷移
        ActorStateSequence battleLoseSequence = new(3);
        ActorSequenceNodeAnimation nodePanicAnimation = new();
        ActorSequenceNodeRun nodeRun = new();
        ActorSequenceNodeAnimation nodeBattleLoseAnimation = new();

        battleLoseSequence.Add(nodePanicAnimation);
        battleLoseSequence.Add(nodeRun);
        battleLoseSequence.Add(nodeBattleLoseAnimation);

        battleLoseSequence.Play(_cts);
    }

    internal ActorStateBase GetState(StateType stateType)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cts.Cancel();
        }
    }
}
