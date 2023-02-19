using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

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
    static readonly int StateDicCapacity = Enum.GetValues(typeof(StateIDOld)).Length;

    ActorStateBase _currentState;
    Dictionary<StateType, ActorStateBase> _stateDic = new(StateDicCapacity);
    IStateControl _stateControl;
    CancellationTokenSource _cts = new CancellationTokenSource();

    public IStateControl StateControl => _stateControl;

    void Awake()
    {
        _stateControl = GetComponent<IStateControl>();

        ActorStateEntry stateEntry = new(this);
        ActorStateExplore stateExplore = new(this);
        ActorStateDead stateDead = new(this);

        _stateDic.Add(StateType.Entry, stateEntry);
        _stateDic.Add(StateType.Explore, stateExplore);
        _stateDic.Add(StateType.Dead, stateDead);

        // うろうろ中に部屋を見つけたら入っていく
        // 一度入った部屋には二度はいらないようにする
        // どうやって部屋を認識するか
        //  ↑部屋かWaypointかをランダムに選択するのは部屋の手前まで来て引き返すので不自然
        //  視界にとらえる必要がある
        //  WaypointにTriggerが必要

        // 1.部屋に入る == 部屋の出入り口に移動する
        // 2.対象を視認、お宝か敵かによってSequenceを変える

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

        //battleLoseSequence.Play(_cts);
    }

    void Start()
    {
        _currentState = GetState(StateType.Entry);
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBase GetState(StateType stateType)
    {
        if (_stateDic.TryGetValue(stateType, out ActorStateBase state))
        {
            return state;
        }
        else
        {
            Debug.LogError("遷移先のステートが辞書内にありません: " + stateType);
            return null;
        }
    }
}
