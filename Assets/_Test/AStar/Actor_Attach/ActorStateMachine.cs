using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A*��p�����L�����N�^�[�̃X�e�[�g�}�V��
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    internal enum StateID
    {
        Idle,
        Move,
        Wander,
        Anim,
        Dead
    }

    // �e�X�e�[�g�͂��̃C���^�[�t�F�[�X�Ŏ�������Ă��郁�\�b�h��K�؂ȃ^�C�~���O�ŌĂяo��
    IActorController _actorController;
    ActorStateBase _currentState;
    Dictionary<StateID, ActorStateBase> _stateDic;

    void Awake()
    {
        // ���I�ɃX�e�[�g��ǉ����Ȃ��̂ŏ����e�ʂ𒴂��邱�Ƃ͂Ȃ�
        int capacity = Enum.GetValues(typeof(StateID)).Length;
        _stateDic = new Dictionary<StateID, ActorStateBase>(capacity);
    }

    void Start()
    {
        _actorController = GetComponent<IActorController>();

        // TOOD:������ӂ̐���������VContainer�ɔC�����Ȃ���
        ActorStateIdle idle      = new ActorStateIdle(_actorController, this);
        ActorStateMove move      = new ActorStateMove(_actorController, this);
        ActorStateWander wander  = new ActorStateWander(_actorController, this);
        ActorStateAnimation anim = new ActorStateAnimation(_actorController, this);
        ActorStateDead dead      = new ActorStateDead(_actorController, this);

        _stateDic.Add(StateID.Idle, idle);
        _stateDic.Add(StateID.Move, move);
        _stateDic.Add(StateID.Wander, wander);
        _stateDic.Add(StateID.Anim, anim);
        _stateDic.Add(StateID.Dead, dead);

        // �o�ꎞ�ɃA�j���[�V�������Đ����邽��
        _currentState = anim;
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBase GetNextState(StateID stateID)
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
