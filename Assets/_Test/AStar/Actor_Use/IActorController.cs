using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����A�e�X�e�[�g�̍s�����J�ڏ�������������C���^�[�t�F�[�X
/// </summary>
public interface IActorController
{
    public bool IsTransitionable();

    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();

    public bool IsTransitionToPanicState();
    public void PlayPanicAnim();

    public void PlayAppearAnim();

    public void PlayWanderAnim();

    public bool IsTransitionToDeadState();
    public void PlayDeadAnim();
}
