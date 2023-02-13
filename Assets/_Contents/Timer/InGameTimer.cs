using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using VContainer;

/// <summary>
/// インゲーム中に使用するタイマーに値を加算するために使う構造体
/// メッセージの送受信のためにint型をラップしたもの
/// </summary>
public struct InGameTimerAddValue
{
    public int Value { get; }

    public InGameTimerAddValue(int value)
    {
        Value = value;
    }
}

/// <summary>
/// インゲーム中に使用するタイマーのコンポーネント
/// </summary>
public class InGameTimer : MonoBehaviour
{
    [Inject]
    readonly ISubscriber<InGameTimerAddValue> _subscriber;

    [Header("インゲームの制限時間(秒)")]
    [SerializeField] int _timeLimit;

    ReactiveProperty<int> _count = new ReactiveProperty<int>();
    int _addValue;

    public IReadOnlyReactiveProperty<int> Count => _count;

    void Start()
    {
        //_subscriber.Subscribe(addValue => 
        //{
        //    AddCount(addValue.Value);
        //}).AddTo(this);
    }

    public async UniTask CountStart(CancellationToken token)
    {
        _count.Value = _timeLimit;

        try
        {
            await Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
                .Select(zeroStartValue =>
                {
                    token.ThrowIfCancellationRequested();

                    int current = (int)(_timeLimit - zeroStartValue);
                    _count.Value = current + _addValue;

                    return _count.Value;
                })
                .TakeWhile(i => i > 0);
        }
        catch(OperationCanceledException e)
        {
            UnityEngine.Debug.Log("インゲームのタイマーの処理がキャンセルされました。: " + e.Message);
        }  
    }

    // 実際にUIに反映されるのは次にOnNext()が発行されたタイミング
    void AddCount(int value) => _addValue += value;
}