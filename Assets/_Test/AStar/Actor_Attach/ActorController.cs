using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] Animator _anim;
    [SerializeField] ActorMove _actorMove;
    [Header("Systemオブジェクトのタグ")]
    [SerializeField] string _tag;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    //bool _isInit;
    bool _isPlayAnim;

    void Start()
    {
        // TODO:この参照先の取得方法がなんか気になる
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        ObservableStateMachineTrigger trigger =
            _anim.GetBehaviour<ObservableStateMachineTrigger>();

        trigger.OnStateEnterAsObservable().Subscribe(state =>
        {
            AnimatorStateInfo info = state.StateInfo;
            if (info.IsName("Sla!sh"))
            {
                // Slashのアニメーションのステートに入った時
                // これを使うことを躊躇しないでください！
            }

        }).AddTo(this);
    }

    public bool IsTransitionIdleState()
    {
        // テスト
        // アニメーションの再生終了後、Idleステートに遷移する
        return !_isPlayAnim; 
    }

    public void MoveToTarget(bool isDash)
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _actorMove.MoveFollowPath(pathStack, isDash);
    }

    public bool IsTransitionMoveState()
    {
        
        return !_isPlayAnim;
    }

    public void MoveCancel()
    {
        _actorMove.MoveCancel();
    }

    public bool IsTransitionAnimationState()
    {
        return false;
        //if (_isInit)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    public void PlayAnim()
    {
        // TODO:優先度(高) アニメーション名を文字列で指定しているのでHashに直す
        _anim.Play("Slash");
        _isPlayAnim = true;
        DOVirtual.DelayedCall(2.0f, () => _isPlayAnim = false);
    }
}
