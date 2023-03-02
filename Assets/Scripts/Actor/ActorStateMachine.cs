using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public enum StateType
{
    Entry,
    Explore,
    Select,
    EnterTheRoom,
    SequenceExecute,
    Goal,
    Dead,
}

public enum SequenceType
{
    Treasure,
    BattleWin,
    BattleLose,
    Exit,
}

/// <summary>
/// キャラクターの行動を制御するステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    static readonly int StateDicCapacity = Enum.GetValues(typeof(StateType)).Length;
    static readonly int SequenceDicCapacity = Enum.GetValues(typeof(SequenceType)).Length;

    ReactiveProperty<ActorStateBase> _currentState = new();
    Dictionary<StateType, ActorStateBase> _stateDic = new(StateDicCapacity);
    Dictionary<SequenceType, ActorStateSequence> _sequenceDic = new(SequenceDicCapacity);
    IStateControl _stateControl;

    public IStateControl StateControl => _stateControl;
    public IReadOnlyReactiveProperty<ActorStateBase> CurrentState => _currentState;

    public void Init()
    {
        _stateControl = GetComponent<IStateControl>();

        ActorStateEntry stateEntry = new(this, StateType.Entry);
        ActorStateExplore stateExplore = new(this, StateType.Explore);
        ActorStateSelect stateSelect = new(this, StateType.Select);
        ActorStateEnterTheRoom stateEnterTheRoom = new(this, StateType.EnterTheRoom);
        ActorStateSequenceExecute stateSequenceExecute = new(this, StateType.SequenceExecute);
        ActorStateGoal stateGoal = new(this, StateType.Goal);
        ActorStateDead stateDead = new(this, StateType.Dead);

        _stateDic.Add(StateType.Entry, stateEntry);
        _stateDic.Add(StateType.Explore, stateExplore);
        _stateDic.Add(StateType.Select, stateSelect);
        _stateDic.Add(StateType.EnterTheRoom, stateEnterTheRoom);
        _stateDic.Add(StateType.SequenceExecute, stateSequenceExecute);
        _stateDic.Add(StateType.Goal, stateGoal);
        _stateDic.Add(StateType.Dead, stateDead);

        // 敵発見時のSequence
        ActorStateSequence sequenceBattleWin = new(length: 5);
        ActorStateSequence sequenceBattleLose = new(length: 4);
        // お宝発見時のSequence
        ActorStateSequence sequenceTreasure = new(length: 5);
        // 体力が閾値以下になった場合に脱出するSequence;
        ActorStateSequence sequenceExit = new(length: 1);

        ActorNodeAffect nodeAffectActorWin = new(this, "ActorWin");
        ActorNodeAffect nodeAffectActorLose = new(this, "ActorLose");
        ActorNodeMoveToTarget nodeMoveToTarget = new(this);
        ActorNodeMoveToExit nodeMoveToExit = new(this);
        ActorNodeAnimation nodePanicAnimation = new(this, "Panic");
        ActorNodeAnimation nodeJoyAnimation = new(this, "Joy");
        ActorNodeAnimation nodeAttackAnimation = new(this, "Attack", iteration: 3);

        sequenceBattleWin.Add(nodePanicAnimation);
        sequenceBattleWin.Add(nodeMoveToTarget);
        sequenceBattleWin.Add(nodeAffectActorWin);
        sequenceBattleWin.Add(nodeAttackAnimation);
        sequenceBattleWin.Add(nodeMoveToExit);

        sequenceBattleLose.Add(nodePanicAnimation);
        sequenceBattleLose.Add(nodeMoveToTarget);
        sequenceBattleLose.Add(nodeAffectActorLose);
        sequenceBattleLose.Add(nodeAttackAnimation);

        sequenceTreasure.Add(nodePanicAnimation);
        sequenceTreasure.Add(nodeMoveToTarget);
        sequenceTreasure.Add(nodeAffectActorWin);
        sequenceTreasure.Add(nodeJoyAnimation);
        sequenceTreasure.Add(nodeMoveToExit);

        sequenceExit.Add(nodeMoveToExit);

        _sequenceDic.Add(SequenceType.BattleWin, sequenceBattleWin);
        _sequenceDic.Add(SequenceType.BattleLose, sequenceBattleLose);
        _sequenceDic.Add(SequenceType.Treasure, sequenceTreasure);
        _sequenceDic.Add(SequenceType.Exit, sequenceExit);

        _currentState.Value = GetState(StateType.Entry);
    }

    public void Execute()
    {
        _currentState.Value = _currentState.Value.Update();
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