using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*��p�����L�����N�^�[�̃X�e�[�g�}�V��
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    // �e�X�e�[�g�͂��̃C���^�[�t�F�[�X�Ŏ�������Ă���
    // ���\�b�h��K�؂ȃ^�C�~���O�ŌĂяo��
    IActorController _movable;

    void Start()
    {
        _movable = GetComponent<IActorController>();

        ActorStateMove actorStateMove = new ActorStateMove(_movable);
        actorStateMove.Update();
    }
}
