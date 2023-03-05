using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面で使用するUIを制御するコンポーネント
/// </summary>
public class ResultUIControl : MonoBehaviour
{
    [SerializeField] HeaderUIAnimation _headerUIAnimation;
    [SerializeField] RawImage _backgroundRawImage;
    [SerializeField] ResultItemAnimation _resultItemAnimation;
    [SerializeField] GameObject _actorStatusRoot;

    void Start()
    {
        _backgroundRawImage.enabled = false;
    }

    public async UniTask AnimationAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _backgroundRawImage.enabled = true;
        _headerUIAnimation.HideAnimation();
        _actorStatusRoot.transform.localScale = Vector3.zero;
        await _resultItemAnimation.AnimationAsync(token);
    }
}
