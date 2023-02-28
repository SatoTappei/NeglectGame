using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// キャラクターから影響を与えられた敵を制御するコンポーネント
/// </summary>
public class EffectableEnemy : SightableObject, IEffectable
{
    static readonly int _idleAnimHash = Animator.StringToHash("Idle");
    static readonly int _attackAnimHash = Animator.StringToHash("Attack");

    [Header("湧いた時に再生されるParticle")]
    [SerializeField] GameObject _popParticle;
    [Header("Attack/Idleの2つのステートを持つアニコン")]
    [SerializeField] Animator _anim;
    [Header("攻撃アニメーションを再生する時間")]
    [SerializeField] float _playingAnimationTime = 4.0f;
    [Header("キャラクターが再び視認できるようになるまでの時間")]
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
            Debug.LogWarning("Enemyでは処理できないメッセージです: " + message);
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
