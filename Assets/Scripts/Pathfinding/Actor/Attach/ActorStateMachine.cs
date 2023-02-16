using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal enum StateID
{
    Non,
    Appear,
    Move,
    Run,
    Attack,
    Joy,
    LookAround,
    Panic,
    Escape,
    Dead
}

/// <summary>
/// A*��p�����L�����N�^�[�̃X�e�[�g�}�V��
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    ActorStateBase _currentState;
    Dictionary<StateID, ActorStateBase> _stateDic;
    IStateControl _stateControl;

    // �e�X�e�[�g�̓C���^�[�t�F�[�X�Ŏ�������Ă��郁�\�b�h��K�؂ȃ^�C�~���O�ŌĂяo��
    internal IStateControl StateControl { get => _stateControl; }

    void Awake()
    {
        // ���I�ɃX�e�[�g��ǉ����Ȃ��̂ŏ����e�ʂ𒴂��邱�Ƃ͂Ȃ�
        int capacity = Enum.GetValues(typeof(StateID)).Length;
        _stateDic = new Dictionary<StateID, ActorStateBase>(capacity);
    }

    void Start()
    {
        _stateControl = GetComponent<IStateControl>();

        // TOOD:������ӂ̐���������VContainer�ɔC�����Ȃ���
        ActorStateAppear appear          = new ActorStateAppear(this);
        ActorStateMove move              = new ActorStateMove(this);
        ActorStateRun run                = new ActorStateRun(this);
        ActorStateAttack attack          = new ActorStateAttack(this);
        ActorStateJoy joy                = new ActorStateJoy(this);
        ActorStateLookAround lookAround  = new ActorStateLookAround(this);
        ActorStatePanic panic            = new ActorStatePanic(this);
        ActorStateEscape escape          = new ActorStateEscape(this);
        ActorStateDead dead              = new ActorStateDead(this);

        // �J�ڐ�ɂ͑I�΂�Ȃ��̂�StateID.Appear�̒ǉ������͂��Ȃ��ŗǂ�
        _stateDic.Add(StateID.Move, move);
        _stateDic.Add(StateID.Run, run);
        _stateDic.Add(StateID.Attack, attack);
        _stateDic.Add(StateID.Joy, joy);
        _stateDic.Add(StateID.LookAround, lookAround);
        _stateDic.Add(StateID.Panic, panic);
        _stateDic.Add(StateID.Escape, escape);
        _stateDic.Add(StateID.Dead, dead);

        // �o�ꎞ�ɃA�j���[�V�������Đ����邽��
        _currentState = appear;
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBase GetState(StateID stateID)
    {
        if (_stateDic.TryGetValue(stateID, out ActorStateBase state))
        {
            return state;
        }
        else
        {
            Debug.LogError("�J�ڐ�̃X�e�[�g���������ɂ���܂���: " + stateID);
            return null;
        }
    }
}