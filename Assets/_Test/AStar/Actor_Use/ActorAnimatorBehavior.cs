using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �X�e�[�g�̑J�ڂ����m���ĉ��o���s���R���|�[�l���g
/// </summary>
public class ActorAnimatorBehavior : StateMachineBehaviour
{
    /* 
     *  TOOD:�L������AI�̃e�X�g�p�ɉ��ō���������Ȃ̂ł�����ƍ�蒼��
     *       �X�e�[�g�ɑJ�ڂ��Ă����Ƃ��ɑΉ������A�C�R����\��������
     */

    enum StateIcon
    {
        Question,
        Exclamation,
    }

    [Header("�J�ڂ����Ƃ��ɕ\�������A�C�R��")]
    [SerializeField] StateIcon _iconType;
    
    //[SerializeField] Sprite _questionIcon;
    //[SerializeField] Sprite _exclamationIcon;

    Text _text;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_text) _text = GameObject.Find("StateText").GetComponent<Text>();

        if(_iconType == StateIcon.Question)
        {
            _text.text = "�^�f";
        }
        else if(_iconType == StateIcon.Exclamation)
        {
            _text.text = "����";
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _text.text = "�ʏ�";
    }
}
