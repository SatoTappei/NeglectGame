using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generator�Ő��������C���X�^���X���Q�Ƃ������R���|�[�l���g�̏������s��
/// ���̃R���|�[�l���g�ɂ���ď���������Ȃ��Ă��Q�[�����̂̓���͂���
/// </summary>
public class GenerateDecorator : MonoBehaviour
{
    /// <summary>Awake()��OnEnable()����AStart()�̑O�ɌĂ΂��</summary>
    public void Decorate(GameObject instance)
    {
        // �ʒu���K�i�ɂ���
        // UI�ɏ��������n��
        Debug.Log("�ł���[���傎");
    }
}
