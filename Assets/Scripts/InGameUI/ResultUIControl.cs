using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// リザルト画面で使用するUIを制御するコンポーネント
/// </summary>
public class ResultUIControl : MonoBehaviour
{
    static float FirstAnimDuration = 0.15f;
    static float SecondAnimDuration = 0.25f;
    static float ThirdAnimDuration = 0.15f;

    [SerializeField] Transform _resultItemRoot;

    void Start()
    {
        _resultItemRoot.transform.localScale = Vector3.zero;
    }

    public async UniTask AnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_resultItemRoot.transform.DOScale(new Vector3(0.01f, 1.0f, 1.0f), FirstAnimDuration));
        sequence.Append(_resultItemRoot.transform.DOScale(new Vector3(1.1f, 1.0f, 1.0f), SecondAnimDuration));
        sequence.Append(_resultItemRoot.transform.DOScale(Vector3.one, ThirdAnimDuration));
        sequence.SetLink(gameObject);

        float delay = FirstAnimDuration + SecondAnimDuration + ThirdAnimDuration;
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);
    }


    // 倒した数を記録する必要がある
    // MVRPで行う
    // ViewはあるがModelとPresenterがない

    // UIに葬った数を表示する
    // リザルトのUIを表示

    // ボタンをクリックしたらタイトルに戻る
    // ボタンの押し込んだアニメーション後にフェードする
    // シーンのリロード
}
