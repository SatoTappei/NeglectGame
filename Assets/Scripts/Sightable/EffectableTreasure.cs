using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// �L�����N�^�[����e����^����ꂽ����𐧌䂷��R���|�[�l���g
/// </summary>
public class EffectableTreasure : EffectableObjectBase
{
    static readonly float TweenDuration = 0.25f;
    static readonly float TweenRotAngle = 120.0f;

    [Header("�J�����Ƃ���Particle")]
    [SerializeField] GameObject _openedParticlePrefab;
    [Header("DOTween�ŃA�j���[�V����������󔠂̊W")]
    [SerializeField] Transform _chestCover;
    [Header("�J���A�j���[�V������̏�����܂ł̎���")]
    [SerializeField] float _lifeTime = 3.0f;
    [Header("�����Ă���ēx�����܂ł̊Ԋu")]
    [SerializeField] float _repopInterval = 8.0f;

    GameObject _openedParticle;

    protected override void InitOnEnable()
    {
        if (!_openedParticle)
        {
            _openedParticle = Instantiate(_openedParticlePrefab, transform.position,
                Quaternion.identity, ParticlePool);
            _openedParticle.SetActive(false);
        }
    }

    protected override void Effect(string message)
    {
        GetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid GetAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _chestCover.DOLocalRotate(new Vector3(0, 0, -TweenRotAngle), TweenDuration)
            .SetEase(Ease.InOutQuad).SetLink(gameObject);
        _openedParticle.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(TweenDuration + _lifeTime));
        
        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(_repopInterval));
        gameObject.SetActive(true);

        _chestCover.DOLocalRotate(new Vector3(0, 0, 0), 0).SetLink(gameObject);
    }
}
