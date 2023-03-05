using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルでの各UIの制御を行うコンポーネント
/// </summary>
public class TitleUIControl : MonoBehaviour
{
    [SerializeField] Button _titleButton;
    [SerializeField] HeaderUIAnimation _headerUIAnim;
    [SerializeField] TitleItemUIAnimation _titleItemUIAnim;

    public async UniTask TitleStateAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        AsyncUnityEventHandler handler = _titleButton.onClick.GetAsyncEventHandler(token);
        await handler.OnInvokeAsync();

        await _titleItemUIAnim.AnimationAsync(token);
        await _headerUIAnim.InAnimationAsync(token);
    }
}
