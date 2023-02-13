using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステートの遷移を検知して演出を行うコンポーネント
/// </summary>
public class ActorAnimatorBehavior : StateMachineBehaviour
{
    /* 
     *  TOOD:キャラのAIのテスト用に仮で作っただけなのできちんと作り直す
     *       ステートに遷移してきたときに対応したアイコンを表示させる
     */

    enum StateIcon
    {
        Question,
        Exclamation,
    }

    [Header("遷移したときに表示されるアイコン")]
    [SerializeField] StateIcon _iconType;
    
    //[SerializeField] Sprite _questionIcon;
    //[SerializeField] Sprite _exclamationIcon;

    Text _text;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_text) _text = GameObject.Find("StateText").GetComponent<Text>();

        if(_iconType == StateIcon.Question)
        {
            _text.text = "疑惑";
        }
        else if(_iconType == StateIcon.Exclamation)
        {
            _text.text = "驚愕";
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _text.text = "通常";
    }
}
