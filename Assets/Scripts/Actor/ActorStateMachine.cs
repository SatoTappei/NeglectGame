using System;
using System.Collections.Generic;
using UnityEngine;

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
/// �L�����N�^�[�̍s���𐧌䂷��X�e�[�g�}�V��
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    static readonly int StateDicCapacity = Enum.GetValues(typeof(StateType)).Length;
    static readonly int SequenceDicCapacity = Enum.GetValues(typeof(SequenceType)).Length;

    ActorStateBase _currentState;
    Dictionary<StateType, ActorStateBase> _stateDic = new(StateDicCapacity);
    Dictionary<SequenceType, ActorStateSequence> _sequenceDic = new(SequenceDicCapacity);
    IStateControl _stateControl;

    public IStateControl StateControl => _stateControl;

    public void Init()
    {
        _stateControl = GetComponent<IStateControl>();

        ActorStateEntry stateEntry = new(this);
        ActorStateExplore stateExplore = new(this);
        ActorStateSelect stateSelect = new(this);
        ActorStateEnterTheRoom stateEnterTheRoom = new(this);
        ActorStateSequenceExecute stateSequenceExecute = new(this);
        ActorStateGoal stateGoal = new(this);
        ActorStateDead stateDead = new(this);

        _stateDic.Add(StateType.Entry, stateEntry);
        _stateDic.Add(StateType.Explore, stateExplore);
        _stateDic.Add(StateType.Select, stateSelect);
        _stateDic.Add(StateType.EnterTheRoom, stateEnterTheRoom);
        _stateDic.Add(StateType.SequenceExecute, stateSequenceExecute);
        _stateDic.Add(StateType.Goal, stateGoal);
        _stateDic.Add(StateType.Dead, stateDead);

        // �G��������Sequence
        ActorStateSequence sequenceBattleWin = new(length: 4);
        ActorStateSequence sequenceBattleLose = new(length: 3);
        // ���󔭌�����Sequence
        ActorStateSequence sequenceTreasure = new(length: 4);
        // �̗͂�臒l�ȉ��ɂȂ����ꍇ�ɒE�o����Sequence;
        ActorStateSequence sequenceExit = new(length: 1);

        ActorNodeMoveToTarget nodeRunToInSightObject = new(this);
        ActorNodeMoveToExit nodeMoveToExit = new(this);
        ActorNodeAnimation nodePanicAnimation = new(this, "Panic");
        ActorNodeAnimation nodeJoyAnimation = new(this, "Joy");
        ActorNodeAnimation nodeAttackAnimation = new(this, "Attack", iteration: 3);

        sequenceBattleWin.Add(nodePanicAnimation);
        sequenceBattleWin.Add(nodeRunToInSightObject);
        sequenceBattleWin.Add(nodeAttackAnimation);
        sequenceBattleWin.Add(nodeMoveToExit);

        sequenceBattleLose.Add(nodePanicAnimation);
        sequenceBattleLose.Add(nodeRunToInSightObject);
        sequenceBattleLose.Add(nodeAttackAnimation);

        sequenceTreasure.Add(nodePanicAnimation);
        sequenceTreasure.Add(nodeRunToInSightObject);
        sequenceTreasure.Add(nodeJoyAnimation);
        sequenceTreasure.Add(nodeMoveToExit);

        sequenceExit.Add(nodeMoveToExit);

        _sequenceDic.Add(SequenceType.BattleWin, sequenceBattleWin);
        _sequenceDic.Add(SequenceType.BattleLose, sequenceBattleLose);
        _sequenceDic.Add(SequenceType.Treasure, sequenceTreasure);
        _sequenceDic.Add(SequenceType.Exit, sequenceExit);

        _currentState = GetState(StateType.Entry);
    }

    public void Execute()
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
            Debug.LogError("�J�ڐ�̃X�e�[�g���������ɂ���܂���: " + type);
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
            Debug.LogError("Sequence���������ɂ���܂���: " + type);
            return null;
        }
    }
}