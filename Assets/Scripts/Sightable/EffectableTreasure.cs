using UnityEngine;
using DG.Tweening;

/// <summary>
/// �L�����N�^�[����e����^����ꂽ����𐧌䂷��R���|�[�l���g
/// </summary>
public class EffectableTreasure : SightableObject, IEffectable
{
    [Header("DOTween�ŃA�j���[�V����������󔠂̊W")]
    [SerializeField] Transform _chestCover;
    [Header("�J���A�j���[�V������̏�����܂ł̎���")]
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
        // �A�j���[�V�����Đ���j��
        _chestCover.DOLocalRotate(new Vector3(0, 0, -120f), 0.25f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Destroy(gameObject, _lifeTime))
            .SetLink(gameObject);
    }
}
