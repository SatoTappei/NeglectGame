using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

/// <summary>
/// インゲーム中に使用するタイマーのテストをするコンポーネント
/// </summary>
public class InGameTimerTestControl : MonoBehaviour
{
    [Inject]
    readonly IPublisher<InGameTimerAddValue> _publisher;

    [SerializeField] InGameTimer _inGameTimer;

    async void Start()
    {
        await _inGameTimer.CountStart(this.GetCancellationTokenOnDestroy());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _publisher.Publish(new InGameTimerAddValue(55));
        }
    }
}
