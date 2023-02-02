using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����A�e�X�e�[�g�̍s�����J�ڏ�������������C���^�[�t�F�[�X
/// </summary>
public interface IStateControl
{
    // �S�ẴX�e�[�g�͊�{���̑J�ڏ����ɏ]��
    public bool IsTransitionable();

    public void MoveToTarget();
    public void RunToTarget();
    public void CancelMoveToTarget();

    public void PlayLookAroundAnim();
    public void PlayAppearAnim();
    public void PlayPanicAnim();
    public void PlayJoyAnim();
    public void PlayAttackAnim();

    // �����̃X�e�[�g����J�ڂ���
    public bool IsTransitionToPanicState();
    public bool IsTransitionToDeadState();

    // �_�b�V�����I�������Ă΂��A�Ȃ񂩃t���O�̃I���I�t�Ƃ�����
    public void RunEndable();

    public void PlayDeadAnim();
}