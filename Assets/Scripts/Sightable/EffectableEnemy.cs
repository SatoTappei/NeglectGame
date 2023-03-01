using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// �L�����N�^�[����e����^����ꂽ�G�𐧌䂷��R���|�[�l���g
/// </summary>
public class EffectableEnemy : EffectableObjectBase
{
    static readonly int _idleAnimHash = Animator.StringToHash("Idle");
    static readonly int _attackAnimHash = Animator.StringToHash("Attack");

    [Header("���񂾂Ƃ���Particle")]
    [SerializeField] GameObject _defeatedParticlePrefab;
    [Header("Attack/Idle��2�̃X�e�[�g�����A�j�R��")]
    [SerializeField] Animator _anim;
    [Header("�U���A�j���[�V�������Đ����鎞��")]
    [SerializeField] float _playingAnimationTime = 4.0f;
    [Header("�L�����N�^�[���Ăю��F�ł���悤�ɂȂ�܂ł̎���")]
    [SerializeField] float _visibleAgainTime = 8.0f;

    GameObject _defeatedParticle;

    protected override void InitOnEnable()
    {
        if (!_defeatedParticle)
        {
            _defeatedParticle = Instantiate(_defeatedParticlePrefab, transform.position,
                Quaternion.identity, ParticlePool);
            _defeatedParticle.SetActive(false);
        }
    }

    protected override void Effect(string message)
    {
        if (message == "ActorWin")
        {
            BattleActorWinAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        else if (message == "ActorLose")
        {
            BattleActorLoseAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        else
        {
            Debug.LogWarning("Enemy�ł͏����ł��Ȃ����b�Z�[�W�ł�: " + message);
        }
    }

    async UniTaskVoid BattleActorWinAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await BattleAnimationAsync(token);

        _defeatedParticle.SetActive(true);

        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(_visibleAgainTime), cancellationToken: token);
        gameObject.SetActive(true);
    }

    async UniTaskVoid BattleActorLoseAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        await BattleAnimationAsync(token);

        float delay = Mathf.Max(0, _visibleAgainTime - _playingAnimationTime);
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);

        // �G���������ꍇ�͔�\���ɂȂ�Ȃ��̂ł����瑤�Ń��Z�b�g�������ĂԕK�v������
        ResetEffectedActor();
    }

    async UniTask BattleAnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _anim.Play(_attackAnimHash);
        await UniTask.Delay(TimeSpan.FromSeconds(_playingAnimationTime), cancellationToken: token);
        _anim.Play(_idleAnimHash);
    }
}
