using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[���s���s������������C���^�[�t�F�[�X
/// </summary>
public interface IActorController
{
    public void MoveStart();
    public void MoveCancel();
    public void PlayAnim();
}
