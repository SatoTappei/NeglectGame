using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �Q�[���S�̗̂���𐧌䂷��R���|�[�l���g
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] DungeonBuilder _dungeonBuilder;

    void Start()
    {
        // �J�����̓N�H�[�^�[�r���[�ŌŒ�

        // �^�C�g�����
        // �{�^�����N���b�N�ŃX�^�[�g
        // �X�e�[�W�������o
        // �t�F�[�h���ă{�^���ƃ��S��������
        // ���o�I����Ƀ^�C�}�[�X�^�[�g
        // n�b�Ԋu�Ŗ`���҂��_���W�����ɂ���Ă���
        //  �K�i�̈ʒu�ɖ`���҂𐶐�
        //  �`���҂̓_���W���������낤�낷��
        // �v���C���[��3��ނ�㩂̂����ǂꂩ��I��ōD���ȂƂ����㩂�u����
        // 㩂͖`���҂Ƀ_���[�W��^����
        // n�b�o������Q�[���I�[�o�[

        // ���U���g
        // ���l�̖`���҂𑒂�����

        // �^�C�g���ɖ߂�

        // �K�v��UI
        // ����:�^�C�}�[
        // �E��:�������`���҂̐�(�X�R�A)
        // �E��:㩗p�̃{�^��3��
        // �E:�e�`���҂̃X�e�[�^�X�A�C�R��5��

        // �_���W�����������ɃA�j���[�V����������̂�Capacity�𑝂₵�Čx��������
        // �������ׂ����ɂȂ����ꍇ�̓A�j���[�V��������߂邱��
        DOTween.SetTweensCapacity(500, 50);
        _dungeonBuilder.Build();


    }

    void Update()
    {
        
    }
}
