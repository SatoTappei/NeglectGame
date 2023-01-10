using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// フェードしながら点滅をするボタン
/// </summary>
public class ExtendFadeButton : ExtendButton
{
    readonly float Duration = 2.0f;

    [SerializeField] Image _img;

    void Start()
    {
        _img.DOFade(0, Duration).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject); 
    }
}
