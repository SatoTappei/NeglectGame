using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 拡張したボタン
/// </summary>
public class ExtendButton : MonoBehaviour, IPointerEnterHandler,
                                           IPointerExitHandler,
                                           IPointerDownHandler,
                                           IPointerUpHandler
{
    readonly float ExpandScale = 1.1f;
    readonly float ShrinkScale = 0.9f;
    readonly float Duration = 0.1f;

    [Header("フェードのアニメーション用のCanvasGroup")]
    [SerializeField] CanvasGroup _canvasGroup;

    void Start()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.DOFade(0.1f, 2.0f).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(ExpandScale, ExpandScale, 1), Duration).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, Duration).SetLink(gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, Duration).SetLink(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(ShrinkScale, ShrinkScale, 1), Duration).SetLink(gameObject);
        AudioManager.Instance.PlaySE("SE_ボタンクリック");
    }
}
