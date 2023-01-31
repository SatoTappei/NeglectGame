using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートの遷移を検知して演出を行うコンポーネント
/// </summary>
public class ActorAnimatorBehavior : StateMachineBehaviour
{
    //[SerializeField] GameObject _prefab;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 何か発見したらビックリマークが出る
        // うろうろ中波？マークが出る
        //Instantiate(_prefab);
    }
}
