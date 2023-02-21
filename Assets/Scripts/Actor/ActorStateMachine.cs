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

        // �G��������Sequence
        ActorStateSequence battleWinSequence = new(length: 4);
        ActorStateSequence battleLoseSequence = new(length: 4);
        // ���󔭌�����Sequence
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


// ���낤�뒆�ɕ�����������������Ă���
// ��x�����������ɂ͓�x�͂���Ȃ��悤�ɂ���
// �ǂ�����ĕ�����F�����邩
//  ��������Waypoint���������_���ɑI������͕̂����̎�O�܂ŗ��Ĉ����Ԃ��̂ŕs���R

// ���C�����ȉ���Sequence
//  �E�o(�ʒu�ɓ���)

// �����𔭌������Ƃ���Sequence
//  �ΏۂɌ������Ĉړ�(�ʒu�ɓ���)
//  �����Ȃ����Explore�X�e�[�g��
//  �������͊eSequence�� <= 

// �󔠂𔭌������Ƃ���Sequence
//  �������A�j���[�V����(�A�j���[�V�����I��)
//  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
//  �l���̃A�j���[�V����(�A�j���[�V�����I��)
//  �o���ɖ߂�(�ʒu�ɓ���)

// �G�𔭌������Ƃ���Sequence(���ʂ�����)
//  �������A�j���[�V����(�A�j���[�V�����I��)
//  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
//  �퓬����(����)(�A�j���[�V�����I��)
//  �o���ɖ߂�(�ʒu�ɓ���)

// �G�𔭌������Ƃ���Sequence(���ʂ�����)
//  �������A�j���[�V����(�A�j���[�V�����I��)
//  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
//  �퓬����(����)(�A�j���[�V�����I��)
//  ���S�X�e�[�g�ɑJ��

/* 
 * ���ۂɏ���Sequence���������Ă݂���
 */
// �eSequence�Ŏg���m�[�h�̃N���X���쐬����
//  ���ꂼ���StateMachine��Sequence��n���Ă���̂ŕ��������͂�