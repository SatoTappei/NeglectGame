using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class TitleItemUIAnimation : MonoBehaviour
{
    static float FirstAnimDuration = 0.15f;
    static float SecondAnimDuration = 0.25f;

    [SerializeField] Transform _titleItemRoot;

    public async UniTask AnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_titleItemRoot.DOScale(Vector3.one * 1.1f, FirstAnimDuration));
        sequence.Append(_titleItemRoot.DOScale(Vector3.zero, SecondAnimDuration));
        sequence.SetLink(gameObject);

        float delay = FirstAnimDuration + SecondAnimDuration;
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);
    }
}
