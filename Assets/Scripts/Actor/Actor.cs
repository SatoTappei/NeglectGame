using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e�R���|�[�l���g�𐧌䂷��R���|�[�l���g
/// </summary>
public class Actor : MonoBehaviour
{
    [SerializeField] ActorMoveSystem _actorMoveSystem;
    [SerializeField] ActorStateMachineOld _actorStateMachine;
    [SerializeField] ActorSight _actorSight;
    [SerializeField] ActorEffecter _actorEffecter;

    /* �����̃^�X�N:Statemachine�̍쐬 */

    // �o�ꉉ�o
    // ���낤��
    //  ���ȉ��̂��C�ŏo���ֈړ�

    // ������������ꍇ
    // ���e�X�e�[�g�ł̒l�̎󂯓n���͂���Ȃ�
    //  �������A�j���[�V����(�A�j���[�V�����I��)
    //  �ΏۂɌ������ă_�b�V��(�ʒu������)
    //  ���(�A�j���[�V�����I��)

    // �G���������ꍇ
    //  �������A�j���[�V����(�A�j���[�V�����I��)
    //  �ΏۂɌ������ă_�b�V��(�ʒu�ɓ���)
    //  �퓬����(���ۂɐ���Ă���킯�ł͂Ȃ��A���̊m���ŏ������������܂�)
    //  ���C��臒l�ȏォ�ǂ�������
    //  �o���ֈړ�

    // �c���[���ւ̎Q��
    // ���C���ǂꂭ�炢��
    // �������邽�߂̎��E

    // �ǂ̏�Ԃ�������˂�
}
