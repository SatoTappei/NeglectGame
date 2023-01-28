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
    [SerializeField] GameObject _testDestroyedPrefab;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    //bool _isInit;
    bool _isPlayAnim;
    bool _isPlayDiscoverAnim;
    bool _isMoving;
    bool _isLookArounding;

    void Start()
    {
        // TODO:この参照先の取得方法がなんか気になる
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();

        // これつかってない
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
        _actorMove.MoveFollowPath(pathStack, isDash, () => _isMoving = false);
        _isMoving = true;
    }

    public bool IsTransitionToWanderStateFromMoveState()
    {
        // 移動が終わったことを検知してうろうろに遷移させる
        return !_isMoving;
    }

    public void MoveCancel()
    {
        _actorMove.MoveCancel();
    }

    public void PlayLookAround()
    {
        _actorMove.LookAround(() => _isLookArounding = false);
        _isLookArounding = true;
    }

    public bool IsTransitionToMoveStateFromWanderStateAfterLookAroundDOtweenAnimation()
    {
        return !_isLookArounding;
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
        // 2秒後にフラグを折っているがこれをアニメーションに合わせる処理が必要
        DOVirtual.DelayedCall(2.0f, () => _isPlayAnim = false);
    }

    public bool IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDiscoverAnim()
    {
        // 発見したときのアニメーションを再生
        _anim.Play("Slash");
        _isPlayDiscoverAnim = true;
        // 2秒後にフラグを折っているがこれをアニメーションに合わせる処理が必要
        DOVirtual.DelayedCall(2.0f, () => _isPlayDiscoverAnim = false);
        // 対象の部屋に向かう(MoveState)
    }

    public void FromAnyStateDead()
    {
        Destroy(gameObject);
        Instantiate(_testDestroyedPrefab, transform.position, Quaternion.identity);
    }

    public bool IsTransitionToAnimationStateFromMoveState()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool IsTransitionToMoveStateFromDiscoverState()
    {
        return !_isPlayDiscoverAnim;
    }
}
