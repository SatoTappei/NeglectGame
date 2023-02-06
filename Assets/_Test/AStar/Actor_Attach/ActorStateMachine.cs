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
    // �e�X�e�[�g�͂��̃C���^�[�t�F�[�X�Ŏ�������Ă��郁�\�b�h��K�؂ȃ^�C�~���O�ŌĂяo��
    IStateControl _stateControl;

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
        ActorStateAppear appear          = new ActorStateAppear(_stateControl, this);
        ActorStateMove move              = new ActorStateMove(_stateControl, this);
        ActorStateRun run                = new ActorStateRun(_stateControl, this);
        ActorStateAttack attack          = new ActorStateAttack(_stateControl, this);
        ActorStateJoy joy                = new ActorStateJoy(_stateControl, this);
        ActorStateLookAround lookAround  = new ActorStateLookAround(_stateControl, this);
        ActorStatePanic panic            = new ActorStatePanic(_stateControl, this);
        ActorStateEscape escape          = new ActorStateEscape(_stateControl, this);
        ActorStateDead dead              = new ActorStateDead(_stateControl, this);

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
