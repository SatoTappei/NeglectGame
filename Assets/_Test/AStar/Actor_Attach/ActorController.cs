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
    readonly int AppearAnimState = Animator.StringToHash("Slash");
    readonly int PanicAnimState = Animator.StringToHash("Slash");

    [SerializeField] Animator _anim;
    [SerializeField] ActorMove _actorMove;
    [Header("Systemオブジェクトのタグ")]
    [SerializeField] string _tag;
    [SerializeField] GameObject _testDestroyedPrefab;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    bool _isPlayAnim;
    bool _isTransitionable;

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
            if (state.StateInfo.IsName("Sla!sh"))
            {
                // Slashのアニメーションのステートに入った時
                // これを使うことを躊躇しないでください！
            }

        }).AddTo(this);
    }

    public void MoveToTarget()
    {
        _isTransitionable = false;
        _actorMove.MoveFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void RunToTarget()
    {
        _isTransitionable = false;
        _actorMove.RunFollowPath(GetPathStack(), () => _isTransitionable = true);
    }

    public void CancelMoveToTarget() => _actorMove.MoveCancel();

    Stack<Vector3> GetPathStack()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        return _pathGetable.GetPathStack(transform.position, targetPos);
    }

    public bool IsTransitionable() => _isTransitionable;

    public void PlayWanderAnim()
    {
        _isTransitionable = false;
        _actorMove.LookAround(() => _isTransitionable = true);
    }

    // ★要リファクタリング
    public void PlayAppearAnim()
    {
        // TODO:優先度(高) アニメーション名を文字列で指定しているのでHashに直す
        _anim.Play(AppearAnimState);
        _isTransitionable = false;
        // 2秒後にフラグを折っているがこれをアニメーションに合わせる処理が必要
        DOVirtual.DelayedCall(2.0f, () => _isTransitionable = true);
    }

    public bool IsTransitionToPanicState()
    {
        // ★何か見つけた
        return Input.GetKeyDown(KeyCode.W);
    }

    // ★要リファクタリング
    public void PlayPanicAnim()
    {
        // 発見したときのアニメーションを再生
        _anim.Play(PanicAnimState);
        _isTransitionable = false;
        // 2秒後にフラグを折っているがこれをアニメーションに合わせる処理が必要
        DOVirtual.DelayedCall(2.0f, () => _isTransitionable = true);
        // 対象の部屋に向かう(MoveState)
    }

    public bool IsTransitionToDeadState()
    {
        // ★死亡を判定する
        return Input.GetKeyDown(KeyCode.Q);
    }

    public void PlayDeadAnim()
    {
        Destroy(gameObject);
        Instantiate(_testDestroyedPrefab, transform.position, Quaternion.identity);
    }
}
