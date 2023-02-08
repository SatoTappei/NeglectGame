using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ダメージを受けるオブジェクト(仮)
/// </summary>
public class DamageReceiver : MonoBehaviour
{
    void OnDestroy()
    {
        // 死んだときにMessagePipeを用いてメッセージを発行する
    }
}
