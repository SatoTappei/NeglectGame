using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// キャラクターから影響を与えられた敵を制御するコンポーネント
/// </summary>
public class EffectableEnemy : EffectableObjectBase
{
    static readonly int _idleAnimHash = Animator.StringToHash("Idle");
    static readonly int _attackAnimHash = Animator.StringToHash("Attack");

    [Header("死んだときのParticle")]
    [SerializeField] GameObject _defeatedParticlePrefab;
    [Header("Attack/Idleの2つのステートを持つアニコン")]
    [SerializeField] Animator _anim;
    [Header("攻撃アニメーションを再生する時間")]
    [SerializeField] float _playingAnimationTime = 4.0f;
    [Header("キャラクターが再び視認できるようになるまでの時間")]
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
            Debug.LogWarning("Enemyでは処理できないメッセージです: " + message);
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

        // 敵が勝った場合は非表示にならないのでこちら側でリセット処理を呼ぶ必要がある
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
