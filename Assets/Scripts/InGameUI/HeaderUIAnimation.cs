using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class HeaderUIAnimation : MonoBehaviour
{
    static float AnimDuration = 0.25f;

    [SerializeField] Transform _headerRoot;

    public async UniTask InAnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _headerRoot.DOMoveY(1080, AnimDuration).SetLink(gameObject);

        await UniTask.Delay(System.TimeSpan.FromSeconds(AnimDuration), cancellationToken: token);
    }

    public void HideAnimation()
    {
        _headerRoot.DOMoveY(1300, AnimDuration).SetLink(gameObject);
    }
}
