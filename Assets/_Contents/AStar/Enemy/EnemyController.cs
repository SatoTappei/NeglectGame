using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�𐧌䂷��R���|�[�l���g
/// </summary>
public class EnemyController : MonoBehaviour
{
    /* 
     *  ����ԁA�{���̓X�e�[�g�}�V���ɂ���ׂ� 
     */

    [SerializeField] GameObject _weapon;

    // ��肤���Ԃ�3��
    // �ҋ@�c�������ɂ͂��̏��
    // �U���c�������ōU�����s��
    // ���S�c��莞�Ԍ�ɕ�������(�o�ꉉ�o���s���đҋ@�ɖ߂�)

    void Start()
    {
        // �ꎞ�I�ȏ����Ƃ��Ĉ��Ԋu�ŕ���̕\��/��\����؂�ւ�����J��Ԃ�
        // ���̃_���[�W��^���鏈��
        InvokeRepeating(nameof(ActiveWeapon), 0, 1);
    }

    void ActiveWeapon()
    {
        _weapon.SetActive(!_weapon.activeInHierarchy);
    }
}
