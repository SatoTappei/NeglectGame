using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;
using UnityEditor.Animations;

//enum AnimType
//{
//    LookAround,
//    Appear,
//    Panic,
//    Attack,
//    Joy,
//}

/// <summary>
/// 経路探索をした結果を用いてキャラクターを移動させるコンポーネント
/// </summary>
public class ActorAction : MonoBehaviour
{
    //[System.Serializable]
    //public class AnimData
    //{
    //    [SerializeField] AnimationClip _clip;
    //    [SerializeField] string _stateName;

    //    public int GetHash() => Animator.StringToHash(_stateName);
    //    public float GetLength() => _clip.length;
    //}

    struct ClipData
    {
        public ClipData(string name,float length)
        {
            Hash = Animator.StringToHash(name);
            Length = length;
        }

        public int Hash { get; }
        public float Length { get; }
    }

    readonly int MoveAnimState = Animator.StringToHash("Move");
    readonly int RunAnimState = Animator.StringToHash("Run");

    public static readonly int hoge;

    [SerializeField] Animator _anim;
    [Header("移動速度")]
    [SerializeField] float _speed;
    [Header("走って移動する時の速度倍率")]
    [SerializeField] float _runSpeedMag = 1.5f;
    [Header("アニメーションの長さ取得用")]
    //[SerializeField] AnimData _lookAroundAnimData;
    //[SerializeField] AnimData _appearAnimData;
    //[SerializeField] AnimData _panicAnimData;
    //[SerializeField] AnimData _attackAnimData;
    //[SerializeField] AnimData _joyAnimData;

    Dictionary<string, ClipData> _animDataDic;

    void Awake()
    {
        RuntimeAnimatorController v = _anim.runtimeAnimatorController;
        AnimatorController anicon = v as AnimatorController;
        
        AnimationClip[] clips = anicon.animationClips;
        ChildAnimatorState[] states = anicon.layers[0].stateMachine.states;

        //Debug.Log(clips.Length + " " + states.Length);

        //var clips = anicon.animationClips;
        //foreach(var viv in clips)
        //{
        //    Debug.Log(viv.name);
        //}
        //var layer = anicon.layers[0];
        //foreach(var vvv in layer.stateMachine.states)
        //{
        //    Debug.Log(vvv.state.name);
        //}

        _animDataDic = new Dictionary<string, ClipData>();
        for (int i = 0; i < 8; i++)
        {
            //Debug.Log(clips[i].length + ":" + states[i].state.name);
            _animDataDic.Add(states[i].state.name, new ClipData(states[i].state.name, clips[i].length));
        }

        //_animDataDic.Add(AnimType.LookAround, _lookAroundAnimData);
        //_animDataDic.Add(AnimType.Appear, _appearAnimData);
        //_animDataDic.Add(AnimType.Panic, _panicAnimData);
        //_animDataDic.Add(AnimType.Attack, _attackAnimData);
        //_animDataDic.Add(AnimType.Joy, _joyAnimData);
    }

    /// <summary>移動開始時にインスタンスのnew、移動のキャンセルには.Cancel()を呼ぶ</summary>
    CancellationTokenSource _token;

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _anim.Play(MoveAnimState);
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _anim.Play(RunAnimState);   
        MoveAsync(stack, _speed * _runSpeedMag, callBack).Forget();
    }

    internal void MoveCancel() => _token?.Cancel();

    async UniTaskVoid MoveAsync(Stack<Vector3> stack, float speed, UnityAction callBack)
    {
        _token = new CancellationTokenSource();

        foreach (Vector3 pos in stack)
        {
            _anim.transform.DOLookAt(pos, 0.5f).SetLink(gameObject);

            while (true)
            {
                if (transform.position == pos) break;

                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
                await UniTask.Yield(cancellationToken: _token.Token);
            }
        }

        callBack.Invoke();
    }

    internal void PlayAnim(string stateName, UnityAction callback)
    {
        if (_animDataDic.TryGetValue(stateName, out ClipData clipLength))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_anim.gameObject.transform.DOLocalMove(Vector3.zero, 0.15f))
                    .AppendCallback(() =>
                    {
                        _anim.Play(clipLength.Hash);
                        DOVirtual.DelayedCall(clipLength.Length, () => callback?.Invoke());
                    })
                    .SetLink(gameObject);
        }
        else
        {
            Debug.LogError("対応するAnimationClipが登録されていません:" + stateName);
        }
    }

    void OnDestroy()
    {
        _token?.Cancel();
    }
}