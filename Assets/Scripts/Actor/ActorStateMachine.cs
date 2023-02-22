using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum StateType
{
    Entry,
    Explore,
    MoveToRoomEntrance,
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
        ActorStateMoveToRoomEntrance stateMoveToRoomEntrance = new(this);
        ActorStateInSightSelect stateInSightSelect = new(this);
        ActorStateSequenceExecute stateSequenceExecute = new(this);
        ActorStateDead stateDead = new(this);

        _stateDic.Add(StateType.Entry, stateEntry);
        _stateDic.Add(StateType.Explore, stateExplore);
        _stateDic.Add(StateType.MoveToRoomEntrance, stateMoveToRoomEntrance);
        _stateDic.Add(StateType.InSightSelect, stateInSightSelect);
        _stateDic.Add(StateType.SequenceExecute, stateSequenceExecute);
        _stateDic.Add(StateType.Dead, stateDead);

        // 敵発見時のSequence
        ActorStateSequence battleWinSequence = new(length: 4);
        ActorStateSequence battleLoseSequence = new(length: 4);
        // お宝発見時のSequence
        ActorStateSequence treasureSequence = new(length: 4);

        ActorNodeRunToInSightObject nodeRunToInSightObject = new(this);
        ActorNodeMoveToExit nodeMoveToExit = new(this);
        ActorNodeAnimation nodePanicAnimation = new(this, "Panic");
        ActorNodeAnimation nodeJoyAnimation = new(this, "Joy");
        ActorNodeAnimation nodeAttackAnimation = new(this, "Attack", iteration: 3);

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