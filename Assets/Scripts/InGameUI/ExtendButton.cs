using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// �g�������{�^��
/// </summary>
public class ExtendButton : MonoBehaviour, IPointerEnterHandler,
                                           IPointerExitHandler,
                                           IPointerDownHandler,
                                           IPointerUpHandler
{
    readonly float ExpandScale = 1.1f;
    readonly float ShrinkScale = 0.9f;
    readonly float Duration = 0.1f;

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
    }
}