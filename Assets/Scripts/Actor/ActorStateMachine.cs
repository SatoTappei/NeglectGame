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
/// �L�����N�^�[�̍s���𐧌䂷��X�e�[�g�}�V��
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

        // ���낤�뒆�ɕ�����������������Ă���
        // ��x�����������ɂ͓�x�͂���Ȃ��悤�ɂ���
        // �ǂ�����ĕ�����F�����邩
        //  ��������Waypoint���������_���ɑI������͕̂����̎�O�܂ŗ��Ĉ����Ԃ��̂ŕs���R
        //  ���E�ɂƂ炦��K�v������
        //  Waypoint��Trigger���K�v

        // 1.�����ɓ��� == �����̏o������Ɉړ�����
        // 2.�Ώۂ����F�A���󂩓G���ɂ����Sequence��ς���

        // ���C�����ȉ���Sequence
        //  �E�o(�ʒu�ɓ���)

        // �󔠂𔭌������Ƃ���Sequence
        //  �������A�j���[�V����(�A�j���[�V�����I��)
        //  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
        //  �l���̃A�j���[�V����(�A�j���[�V�����I��)

        // �G�𔭌������Ƃ���Sequence(���ʂ�����)
        //  �������A�j���[�V����(�A�j���[�V�����I��)
        //  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
        //  �퓬����(����)(�A�j���[�V�����I��)

        // �G�𔭌������Ƃ���Sequence(���ʂ�����)
        //  �������A�j���[�V����(�A�j���[�V�����I��)
        //  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
        //  �퓬����(����)(�A�j���[�V�����I��)
        //  ���S�X�e�[�g�ɑJ��
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
            Debug.LogError("�J�ڐ�̃X�e�[�g���������ɂ���܂���: " + stateType);
            return null;
        }
    }
}
