using UnityEngine;
using DG.Tweening;

/// <summary>
/// キャラクターから影響を与えられたお宝を制御するコンポーネント
/// </summary>
public class EffectableTreasure : SightableObject, IEffectable
{
    [Header("DOTweenでアニメーションさせる宝箱の蓋")]
    [SerializeField] Transform _chestCover;
    [Header("開くアニメーション後の消えるまでの時間")]
    [SerializeField] float _lifeTime = 3.0f;

    Actor _effectedActor;

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
        // アニメーション再生後破棄
        _chestCover.DOLocalRotate(new Vector3(0, 0, -120f), 0.25f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Destroy(gameObject, _lifeTime))
            .SetLink(gameObject);
    }
}
