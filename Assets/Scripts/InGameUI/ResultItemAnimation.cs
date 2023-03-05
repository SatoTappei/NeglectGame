using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class ResultItemAnimation : MonoBehaviour
{
    static float FirstAnimDuration = 0.15f;
    static float SecondAnimDuration = 0.25f;
    static float ThirdAnimDuration = 0.15f;

    [SerializeField] Transform _resultItem;

    void Start()
    {
        _resultItem.transform.localScale = Vector3.zero;
    }

    public async UniTask AnimationAsync(CancellationToken token)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_resultItem.transform.DOScale(new Vector3(0.01f, 1.0f, 1.0f), FirstAnimDuration));
        sequence.Append(_resultItem.transform.DOScale(new Vector3(1.1f, 1.0f, 1.0f), SecondAnimDuration));
        sequence.Append(_resultItem.transform.DOScale(Vector3.one, ThirdAnimDuration));
        sequence.SetLink(gameObject);

        float delay = FirstAnimDuration + SecondAnimDuration + ThirdAnimDuration;
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);
    }
}
