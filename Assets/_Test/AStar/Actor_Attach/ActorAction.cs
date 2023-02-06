using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;

enum AnimType
{
    LookAround,
    Appear,
    Panic,
    Attack,
    Joy,
}

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorAction : MonoBehaviour
{
    [System.Serializable]
    public class AnimData
    {
        [SerializeField] AnimationClip _clip;
        [SerializeField] string _stateName;

        public int GetHash() => Animator.StringToHash(_stateName);
        public float GetLength() => _clip.length;
    }

    readonly int MoveAnimState = Animator.StringToHash("Move");
    readonly int RunAnimState = Animator.StringToHash("Run");

    [SerializeField] Animator _anim;
    [Header("�ړ����x")]
    [SerializeField] float _speed;
    [Header("�����Ĉړ����鎞�̑��x�{��")]
    [SerializeField] float _runSpeedMag = 1.5f;
    [Header("�A�j���[�V�����̒����擾�p")]
    [SerializeField] AnimData _lookAroundAnimData;
    [SerializeField] AnimData _appearAnimData;
    [SerializeField] AnimData _panicAnimData;
    [SerializeField] AnimData _attackAnimData;
    [SerializeField] AnimData _joyAnimData;

    Dictionary<AnimType, AnimData> _animDataDic;

    void Awake()
    {
        _animDataDic = new Dictionary<AnimType, AnimData>();
        _animDataDic.Add(AnimType.LookAround, _lookAroundAnimData);
        _animDataDic.Add(AnimType.Appear, _appearAnimData);
        _animDataDic.Add(AnimType.Panic, _panicAnimData);
        _animDataDic.Add(AnimType.Attack, _attackAnimData);
        _animDataDic.Add(AnimType.Joy, _joyAnimData);
    }

    /// <summary>�ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�</summary>
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

    internal void PlayAnim(AnimType type, UnityAction callback)
    {
        if(_animDataDic.TryGetValue(type,out AnimData clipData))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_anim.gameObject.transform.DOLocalMove(Vector3.zero, 0.15f))
                    .AppendCallback(() =>
                    {
                        _anim.Play(clipData.GetHash());
                        DOVirtual.DelayedCall(clipData.GetLength(), () => callback?.Invoke());
                    })
                    .SetLink(gameObject);
        }
        else
        {
            Debug.LogError("�Ή�����AnimationClip���o�^����Ă��܂���:" + type);
        }
    }

    void OnDestroy()
    {
        _token?.Cancel();
    }
}