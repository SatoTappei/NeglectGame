using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����A�e�X�e�[�g�̍s�����J�ڏ�������������C���^�[�t�F�[�X
/// </summary>
public interface IActorController
{
    // �S�ẴX�e�[�g�͊�{���̑J�ڏ����ɏ]��
    public bool IsTransitionable();

    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();

    public void PlayWanderAnim();
    public void PlayAppearAnim();
    public void PlayPanicAnim();

    // �����̃X�e�[�g����J�ڂ���
    public bool IsTransitionToPanicState();
    public bool IsTransitionToDeadState();

    public void PlayDeadAnim();
}