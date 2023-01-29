using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���ő��삷�邽�߂Ɏg���A�L�����N�^�[�̍s������������C���^�[�t�F�[�X
/// </summary>
public interface IActorController
{
    public void MoveToTarget();
    public void RunToTarget();
    public bool IsTransitionable();

    public bool IsTransitionToPanicState();
    public void CancelMoveToTarget();

    public void PlayAppearAnim();

    public void PlayWanderAnim();

    public bool IsTransitionToDeadState();

    public void PlayPanicAnim();

    public void PlayDeadAnim();
}
