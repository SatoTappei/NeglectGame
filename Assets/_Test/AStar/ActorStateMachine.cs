using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*��p�����L�����N�^�[�̃X�e�[�g�}�V��
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    [SerializeField] PathfindingPresenter _pathfindingPresenter;

    void Start()
    {
        // �ӗ~�������Ȃ����ꍇ�̓_���W��������E�o����
        // �ړI��B�������ꍇ���_���W��������E�o����
        //  �ړI = �ړI�̕����ɓ��B���邱��
        //  �{�X�����ɓ��B�A���󕔉��ɓ��B�A���낤�낵�������ŋA��
        // �e���}�b�v�ɏ]���čs������

        // �_���W�����ɂ���Ă���
        // �꒼���ŖړI�̕����Ɍ������̂͂������� <= �_���W�����ł��낤�낳����
        // ���낤��c�_���W�����̃����_���ȉӏ��Ɍ������悤�ɂ���
        //           ���������玟�̃����_���ȉӏ��Ɍ�����
        // ���낤�뒆��ړI�̕������������炻�̕����ɓ����Ă���


    }

    void Update()
    {
        
    }
}
