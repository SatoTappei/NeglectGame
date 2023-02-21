using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum StateType
{
    Entry,
    Explore,
    MoveToRoom,
    InSightSelect,
    SequenceExecute,
    Dead,
}

public enum SequenceType
{
    Treasure,
    BattleWin,
    BattleLose,
}

/// <summary>
/// キャラクターの行動を制御するステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    static readonly int StateDicCapacity = Enum.GetValues(typeof(StateType)).Length;
    static readonly int SequenceDicCapacity = Enum.GetValues(typeof(SequenceType)).Length;

    ActorStateBase _currentState;
    Dictionary<StateType, ActorStateBase> _stateDic = new(StateDicCapacity);
    Dictionary<SequenceType, ActorStateSequence> _sequenceDic = new(SequenceDicCapacity);
    IStateControl _stateControl;
    CancellationTokenSource _cts = new CancellationTokenSource();

    public IStateControl StateControl => _stateControl;

    void Awake()
    {
        _stateControl = GetComponent<IStateControl>();

        ActorStateEntry stateEntry = new(this);
        ActorStateExplore stateExplore = new(this);
        ActorStateMoveToRoom stateMoveToRoom = new(this);
        ActorStateInSightSelect stateInSightSelect = new(this);
        ActorStateSequenceExecute stateSequenceExecute = new(this);
        ActorStateDead stateDead = new(this);

        _stateDic.Add(StateType.Entry, stateEntry);
        _stateDic.Add(StateType.Explore, stateExplore);
        _stateDic.Add(StateType.MoveToRoom, stateMoveToRoom);
        _stateDic.Add(StateType.InSightSelect, stateInSightSelect);
        _stateDic.Add(StateType.SequenceExecute, stateSequenceExecute);
        _stateDic.Add(StateType.Dead, stateDead);

        // 敵発見時のSequence
        ActorStateSequence battleWinSequence = new(length: 4);
        ActorStateSequence battleLoseSequence = new(length: 4);
        // お宝発見時のSequence
        ActorStateSequence treasureSequence = new(length: 4);

        ActorNodeRunToInSightObject nodeRunToInSightObject = new(this, battleWinSequence);
        ActorNodeMoveToExit nodeMoveToExit = new(this, battleWinSequence);
        ActorNodeAnimation nodePanicAnimation = new(this, battleWinSequence, "Panic");
        ActorNodeAnimation nodeJoyAnimation = new(this, battleWinSequence, "Joy");
        ActorNodeAnimation nodeAttackAnimation = new(this, battleWinSequence, "Attack");

        battleWinSequence.Add(nodePanicAnimation);
        battleWinSequence.Add(nodeRunToInSightObject);
        battleWinSequence.Add(nodeAttackAnimation);
        battleWinSequence.Add(nodeMoveToExit);

        treasureSequence.Add(nodePanicAnimation);
        treasureSequence.Add(nodeRunToInSightObject);
        treasureSequence.Add(nodeJoyAnimation);
        treasureSequence.Add(nodeMoveToExit);

        _sequenceDic.Add(SequenceType.BattleWin, battleWinSequence);
        _sequenceDic.Add(SequenceType.BattleLose, battleLoseSequence);
        _sequenceDic.Add(SequenceType.Treasure, treasureSequence);
    }

    void Start()
    {
        _currentState = GetState(StateType.Entry);
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBase GetState(StateType type)
    {
        if (_stateDic.TryGetValue(type, out ActorStateBase state))
        {
            return state;
        }
        else
        {
            Debug.LogError("遷移先のステートが辞書内にありません: " + type);
            return null;
        }
    }

    internal ActorStateSequence GetSequence(SequenceType type)
    {
        if (_sequenceDic.TryGetValue(type, out ActorStateSequence sequence))
        {
            return sequence;
        }
        else
        {
            Debug.LogError("Sequenceが辞書内にありません: " + type);
            return null;
        }
    }
}


// うろうろ中に部屋を見つけたら入っていく
// 一度入った部屋には二度はいらないようにする
// どうやって部屋を認識するか
//  ↑部屋かWaypointかをランダムに選択するのは部屋の手前まで来て引き返すので不自然

// やる気が一定以下のSequence
//  脱出(位置に到着)

// 部屋を発見したときのSequence
//  対象に向かって移動(位置に到着)
//  何もなければExploreステートへ
//  もしくは各Sequenceへ <= 

// 宝箱を発見したときのSequence
//  見つけたアニメーション(アニメーション終了)
//  対象に向かってダッシュ(位置に到着)
//  獲得のアニメーション(アニメーション終了)
//  出口に戻る(位置に到着)

// 敵を発見したときのSequence(結果が勝ち)
//  見つけたアニメーション(アニメーション終了)
//  対象に向かってダッシュ(位置に到着)
//  戦闘する(勝ち)(アニメーション終了)
//  出口に戻る(位置に到着)

// 敵を発見したときのSequence(結果が負け)
//  見つけたアニメーション(アニメーション終了)
//  対象に向かってダッシュ(位置に到着)
//  戦闘する(負け)(アニメーション終了)
//  死亡ステートに遷移

/* 
 * 実際に書くSequenceを実装してみある
 */
// 各Sequenceで使うノードのクラスを作成した
//  それぞれにStateMachineとSequenceを渡しているので幅が利くはず