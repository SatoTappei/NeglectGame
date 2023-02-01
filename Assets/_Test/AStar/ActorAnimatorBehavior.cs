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
    
    [SerializeField] Sprite _questionIcon;
    [SerializeField] Sprite _exclamationIcon;

    Image _icon;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_icon) _icon = GameObject.Find("PlayerStateTestIcon").GetComponent<Image>();

        if(_iconType == StateIcon.Question)
        {
            _icon.sprite = _questionIcon;
        }
        else if(_iconType == StateIcon.Exclamation)
        {
            _icon.sprite = _exclamationIcon;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _icon.sprite = null;
    }
}
