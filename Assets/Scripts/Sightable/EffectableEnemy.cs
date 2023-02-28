using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// �L�����N�^�[����e����^����ꂽ�G�𐧌䂷��R���|�[�l���g
/// </summary>
public class EffectableEnemy : SightableObject, IEffectable
{
    static readonly int _idleAnimHash = Animator.StringToHash("Idle");
    static readonly int _attackAnimHash = Animator.StringToHash("Attack");

    [Header("�N�������ɍĐ������Particle")]
    [SerializeField] GameObject _popParticle;
    [Header("Attack/Idle��2�̃X�e�[�g�����A�j�R��")]
    [SerializeField] Animator _anim;
    [Header("�U���A�j���[�V�������Đ����鎞��")]
    [SerializeField] float _playingAnimationTime = 4.0f;
    [Header("�L�����N�^�[���Ăю��F�ł���悤�ɂȂ�܂ł̎���")]
    [SerializeField] float _visibleAgainTime = 8.0f;

    GameObject _particle;
    Actor _effectedActor;

    void OnEnable()
    {
        if (_particle == null)
        {
            _particle = Instantiate(_popParticle, transform.position, Quaternion.identity);
        }
        else
        {
            _particle.SetActive(true);
        }

        _effectedActor = null;
    }

    public override bool IsAvailable(Actor actor)
    {
        if(_effectedActor == null)
        {
            _effectedActor = actor;
            return true;
        }
        else if (_effectedActor == actor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void IEffectable.Effect(string message)
    {
        if (message == "ActorWin")
        {
            BattleWinAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        else if (message == "ActorLose")
        {
            BattleLoseAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        else
        {
            Debug.LogWarning("Enemy�ł͏����ł��Ȃ����b�Z�[�W�ł�: " + message);
        }
    }

    async UniTaskVoid BattleWinAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await BattleAnimationAsync(token);
        
        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(_visibleAgainTime), cancellationToken: token);
        gameObject.SetActive(true);
    }

    async UniTaskVoid BattleLoseAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await BattleAnimationAsync(token);

        float delay = Mathf.Max(0, _visibleAgainTime - _playingAnimationTime);
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);

        _effectedActor = null;
    }

    async UniTask BattleAnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _anim.Play(_attackAnimHash);
        await UniTask.Delay(TimeSpan.FromSeconds(_playingAnimationTime), cancellationToken: token);
        _anim.Play(_idleAnimHash);
    }
}
