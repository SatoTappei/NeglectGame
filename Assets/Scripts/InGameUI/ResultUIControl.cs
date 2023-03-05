using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面で使用するUIを制御するコンポーネント
/// </summary>
public class ResultUIControl : MonoBehaviour
{
    static float FirstAnimDuration = 0.15f;
    static float SecondAnimDuration = 0.25f;
    static float ThirdAnimDuration = 0.15f;

    [SerializeField] HeaderUIAnimation _headerUIAnimation;
    [SerializeField] RawImage _backgroundRawImage;
    [SerializeField] Transform _resultItem;

    void Start()
    {
        _resultItem.transform.localScale = Vector3.zero;
        _backgroundRawImage.enabled = false;
    }

    public async UniTask AnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _backgroundRawImage.enabled = true;

        _headerUIAnimation.HideAnimation();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_resultItem.transform.DOScale(new Vector3(0.01f, 1.0f, 1.0f), FirstAnimDuration));
        sequence.Append(_resultItem.transform.DOScale(new Vector3(1.1f, 1.0f, 1.0f), SecondAnimDuration));
        sequence.Append(_resultItem.transform.DOScale(Vector3.one, ThirdAnimDuration));
        sequence.SetLink(gameObject);

        float delay = FirstAnimDuration + SecondAnimDuration + ThirdAnimDuration;
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);
    }
}
