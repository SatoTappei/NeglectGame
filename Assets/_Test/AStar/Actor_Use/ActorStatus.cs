using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e��X�e�[�^�X�̃f�[�^�݂̂����N���X
/// </summary>
internal class ActorStatus
{
    int _hp;
    GameObject _treasure;

    public int Hp { get => _hp; set => _hp = value; }
    public GameObject Treasure { get => _treasure; set => _treasure = value; }

    /* 
    *  ���܂ł͊e�퓮�������R���|�[�l���g���Ή���������̒l��ێ����Ă���
    *  ���̃N���X�Ƀv���C���[�̃X�e�[�^�X�Ȃǂ�ێ����Ă���
    *  ��������o����悤�ɂ���̂��x�X�g�H
    *  �f�����b�g�Ƃ��Ă̓N���X�Ԃ̌����������Ȃ��Ă��܂�
    */
}
