using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����������ʂ�p���ăL�����N�^�[���ړ�������R���|�[�l���g
/// </summary>
public class ActorAction : MonoBehaviour
{
    // TODO: �A�j���[�V�����̗񋓌^������ActorController���̃��t�@�N�^�����O���o�������H
    //       �K�؂ɍs����ActorController�������������X�b�L��������
    //       ��������ƃX�e�[�g����readonly�ɂ��邱�Ƃ��o���Ȃ�
    //       AnimationClip�A�X�e�[�g��(�n�b�V���p)�A�Ăяo���̗񋓌^������ĂЂƂ܂Ƃ߂ɂ���ׂ��H  

    readonly int WalkAnimState = Animator.StringToHash("Walk");
    readonly int SprintAnimState = Animator.StringToHash("Sprint");
    readonly int LookAroundAnimState = Animator.StringToHash("LookAround");
    readonly int AppearAnimState = Animator.StringToHash("Appear");
    readonly int PanicAnimState = Animator.StringToHash("Jump");

    [SerializeField] Animator _anim;
    [Header("�ړ����x")]
    [SerializeField] float _speed;
    [Header("�����Ĉړ����鎞�̑��x�{��")]
    [SerializeField] float _runSpeedMag = 1.5f;
    [Header("�A�j���[�V�����̒����擾�p")]
    [SerializeField] AnimationClip _lookAroundAnimClip;
    [SerializeField] AnimationClip _appearAnimClip;
    [SerializeField] AnimationClip _panicAnimClip;

    /// <summary>�ړ��J�n���ɃC���X�^���X��new�A�ړ��̃L�����Z���ɂ�.Cancel()���Ă�</summary>
    CancellationTokenSource _token;

    internal void MoveFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _anim.Play(WalkAnimState);
        MoveAsync(stack, _speed, callBack).Forget();
    }

    internal void RunFollowPath(Stack<Vector3> stack, UnityAction callBack)
    {
        _anim.Play(SprintAnimState);   
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

    internal void PlayLookAroundAnim(UnityAction callback)
        => PlayAnim(LookAroundAnimState, callback, _lookAroundAnimClip);
    internal void PlayAppearAnim(UnityAction callback) 
        => PlayAnim(AppearAnimState, callback, _appearAnimClip);
    internal void PlayPanicAnim(UnityAction callback)
        => PlayAnim(PanicAnimState, callback, _panicAnimClip);

    void PlayAnim(int hash, UnityAction callback, AnimationClip clip)
    {
        // TODO: �A�j���[�V�����̒����̎擾���������Y��ɂ܂Ƃ߂���
        //       ���݂͍Đ�����A�j���[�V�������C���X�y�N�^�[���犄�蓖�ĂĂ��̒����������x�������Ă���

        // �A�j���[�V��������x�Ƀ��f���̈ʒu���������Y���Ă����̂ōĐ����邽�т�0�ɖ߂�����������ł���
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_anim.gameObject.transform.DOLocalMove(Vector3.zero, 0.15f))
                .AppendCallback(() => 
                {
                    _anim.Play(hash);
                    DOVirtual.DelayedCall(clip.length, () => callback?.Invoke());
                })
                .SetLink(gameObject);
    }

    void OnDestroy()
    {
        _token?.Cancel();
    }
}