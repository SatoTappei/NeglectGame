using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// キャラクターから影響を与えられたお宝を制御するコンポーネント
/// </summary>
public class EffectableTreasure : EffectableObjectBase
{
    static readonly float TweenDuration = 0.25f;
    static readonly float TweenRotAngle = 120.0f;

    [Header("DOTweenでアニメーションさせる宝箱の蓋")]
    [SerializeField] Transform _chestCover;
    [Header("開くアニメーション後の消えるまでの時間")]
    [SerializeField] float _lifeTime = 3.0f;
    [Header("消えてから再度沸くまでの間隔")]
    [SerializeField] float _repopInterval = 8.0f;

    protected override void Effect(string message)
    {
        GetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid GetAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _chestCover.DOLocalRotate(new Vector3(0, 0, -TweenRotAngle), TweenDuration)
            .SetEase(Ease.InOutQuad).SetLink(gameObject);
        await UniTask.Delay(TimeSpan.FromSeconds(TweenDuration + _lifeTime));
        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(_repopInterval));
        gameObject.SetActive(true);
        _chestCover.DOLocalRotate(new Vector3(0, 0, TweenRotAngle), 0).SetLink(gameObject);
    }
}
