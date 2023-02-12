using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

/// <summary>
/// �_���[�W���󂯂�I�u�W�F�N�g(��)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class DamageReceiver : MonoBehaviour
{
    /* 
     *  TODO:MessagePipe���g�������b�Z�[�W���O�ɒ����Ȃ�����������
     *       �������邱�Ƃł肶�ڂ�����Ȃ����Ȃ�
     */

    int _hp = 9;

    void Awake()
    {
        // �~�X�h�~�̂��߂肶�ڂ̊e�퍀�ڂ��������ŘM��
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 1;
        rb.angularDrag = 0;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // TOOD:��x�̍U���ŕ�����̃q�b�g���������Ă��܂��̂�interval��݂����肷��K�v������
        if (other.gameObject.CompareTag("Weapon"))
        {
            _hp--;
            Debug.Log("�q�b�g");
            if (_hp == 0)
            {
                MessageBroker.Default.Publish(new DamageData());

                Destroy(gameObject);

            }
        }
    }

    void OnDestroy()
    {
        // ���񂾂Ƃ���MessagePipe��p���ă��b�Z�[�W�𔭍s����
        // �_���[�W��^����R���|�l�����\�b�h������Ȃ��Ƃ肶�ڂ�����
    }
}
