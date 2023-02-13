using UniRx;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ダメージを受けた際に発行されるメッセージを受け取るコンポーネント
/// </summary>
public class DamagedMessageReceiver : MonoBehaviour
{
    public UnityAction OnDefeated;
}
