using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���ő��삷�邽�߂Ɏg���A�L�����N�^�[�̍s������������C���^�[�t�F�[�X
/// </summary>
public interface IActorController
{
    public void MoveToTarget(bool isDash);
    public bool IsTransionMoveState();
    public void MoveCancel();

    public bool IsTransionAnimationState();
    public void PlayAnim();
}
