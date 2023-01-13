using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �_���W�����������̃v���n�u�̃A�j���[�V����(��)
/// </summary>
public class GeneratedAnimation : MonoBehaviour
{
    void Start()
    {
        transform.localScale = Vector3.zero;

        float duration = Random.Range(0.5f, 1.0f);
        float delay = Random.value * 0.1f;
        transform.DOScale(Vector3.one, duration)
                 .SetEase(Ease.OutBounce)
                 .SetDelay(delay)
                 .SetLink(gameObject);
    }
}
