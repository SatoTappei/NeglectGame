using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// キャラクターから影響を与えられたお宝を制御するコンポーネント
/// </summary>
public class EffectableTreasure : SightableObject, IEffectable
{
    static readonly float TweenDuration = 0.25f;
    static readonly float TweenRotAngle = 120.0f;

    [Header("湧いた時に再生されるParticle")]
    [SerializeField] GameObject _popParticle;
    [Header("DOTweenでアニメーションさせる宝箱の蓋")]
    [SerializeField] Transform _chestCover;
    [Header("開くアニメーション後の消えるまでの時間")]
    [SerializeField] float _lifeTime = 3.0f;
    [Header("消えてから再度沸くまでの間隔")]
    [SerializeField] float _repopInterval = 8.0f;

    GameObject _particle;
    Actor _effectedActor;
    CancellationTokenSource _cts;

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

        _chestCover.DOLocalRotate(new Vector3(0, 0, TweenRotAngle), 0).SetLink(gameObject);
        _effectedActor = null;
    }

    void OnDisable()
    {
        _cts?.Cancel();
    }

    public override bool IsAvailable(Actor actor)
    {
        if (_effectedActor == null)
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

    void IEffectable.Effect(string _)
    {
        _cts = new CancellationTokenSource();
        GetAsync(_cts).Forget();
    }

    async UniTaskVoid GetAsync(CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        _chestCover.DOLocalRotate(new Vector3(0, 0, -TweenRotAngle), TweenDuration)
            .SetEase(Ease.InOutQuad).SetLink(gameObject);
        await UniTask.Delay(TimeSpan.FromSeconds(TweenDuration + _lifeTime));
        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(_repopInterval));
        gameObject.SetActive(true);
    }
}
