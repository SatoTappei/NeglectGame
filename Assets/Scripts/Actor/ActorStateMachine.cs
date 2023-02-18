using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

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
    CancellationTokenSource _cts = new CancellationTokenSource();

    void Start()
    {
        // ��X�e�[�g
        //  �����_����Waypoint�Ɍ����Ĉړ�����

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

        battleLoseSequence.Play(_cts);
    }

    internal ActorStateBase GetState(StateType stateType)
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _cts.Cancel();
        }
    }
}
