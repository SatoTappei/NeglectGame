using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�̑J�ڂ����m���ĉ��o���s���R���|�[�l���g
/// </summary>
public class ActorAnimatorBehavior : StateMachineBehaviour
{
    //[SerializeField] GameObject _prefab;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ��������������r�b�N���}�[�N���o��
        // ���낤�뒆�g�H�}�[�N���o��
        //Instantiate(_prefab);
    }
}
