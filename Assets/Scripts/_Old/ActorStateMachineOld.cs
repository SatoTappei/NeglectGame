using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateIDOld
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
public class ActorStateMachineOld : MonoBehaviour
{
    ActorStateBaseOld _currentState;
    Dictionary<StateIDOld, ActorStateBaseOld> _stateDic;
    IStateControl _stateControl;

    // �e�X�e�[�g�̓C���^�[�t�F�[�X�Ŏ�������Ă��郁�\�b�h��K�؂ȃ^�C�~���O�ŌĂяo��
    internal IStateControl StateControl { get => _stateControl; }

    void Awake()
    {
        // ���I�ɃX�e�[�g��ǉ����Ȃ��̂ŏ����e�ʂ𒴂��邱�Ƃ͂Ȃ�
        int capacity = Enum.GetValues(typeof(StateIDOld)).Length;
        _stateDic = new Dictionary<StateIDOld, ActorStateBaseOld>(capacity);
    }

    void Start()
    {
        _stateControl = GetComponent<IStateControl>();

        // TOOD:������ӂ̐���������VContainer�ɔC�����Ȃ���
        ActorStateAppear appear          = new ActorStateAppear(this);
        ActorStateMoveOld move              = new ActorStateMoveOld(this);
        ActorStateRun run                = new ActorStateRun(this);
        ActorStateAttack attack          = new ActorStateAttack(this);
        ActorStateJoy joy                = new ActorStateJoy(this);
        ActorStateLookAround lookAround  = new ActorStateLookAround(this);
        ActorStatePanic panic            = new ActorStatePanic(this);
        ActorStateEscape escape          = new ActorStateEscape(this);
        ActorStateDeadOld dead              = new ActorStateDeadOld(this);

        // �J�ڐ�ɂ͑I�΂�Ȃ��̂�StateID.Appear�̒ǉ������͂��Ȃ��ŗǂ�
        _stateDic.Add(StateIDOld.Move, move);
        _stateDic.Add(StateIDOld.Run, run);
        _stateDic.Add(StateIDOld.Attack, attack);
        _stateDic.Add(StateIDOld.Joy, joy);
        _stateDic.Add(StateIDOld.LookAround, lookAround);
        _stateDic.Add(StateIDOld.Panic, panic);
        _stateDic.Add(StateIDOld.Escape, escape);
        _stateDic.Add(StateIDOld.Dead, dead);

        // �o�ꎞ�ɃA�j���[�V�������Đ����邽��
        _currentState = appear;
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBaseOld GetState(StateIDOld stateID)
    {
        if (_stateDic.TryGetValue(stateID, out ActorStateBaseOld state))
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
