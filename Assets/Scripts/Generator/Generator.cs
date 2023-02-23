using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using MessagePipe;
using VContainer;

/// <summary>
/// ActorGeneratorに生成を停止/再開するようメッセージの送受信するのに使う構造体
/// </summary>
public struct GeneratorControl
{
    public bool IsGeneratable { get; }

    public GeneratorControl(bool isGeneratable)
    {
        IsGeneratable = isGeneratable;
    }
}

/// <summary>
/// 冒険者を定期的に生成するジェネレータのコンポーネント
/// </summary>
public class Generator : MonoBehaviour
{
    [Inject]
    readonly ISubscriber<GeneratorControl> _subscriber;

    [Header("生成するプレハブ")]
    [SerializeField] GameObject[] _prefabs;
    [Header("生成したプレハブを登録する親")]
    [SerializeField] Transform _parent;
    [Header("生成する間隔")]
    [SerializeField] float _interval = 1.0f;

    bool _isGeneratable = true;
    /// <summary>
    /// 外部からキャンセル用のメソッドを呼ぶだけで生成処理をキャンセルできるように
    /// CancellationTokenSourceをこちら側で保持しておく
    /// </summary>
    CancellationTokenSource _cts;
    /// <summary>
    /// オブジェクトを生成したタイミングで
    /// 外部からそのオブジェクトを初期化できるように保持しておく
    /// </summary>
    ReactiveProperty<GameObject> _lastInstantiatedPrefab = new ();

    public IReadOnlyReactiveProperty<GameObject> LastInstantiatedPrefab => _lastInstantiatedPrefab;

    void Start()
    {
        // 外部からGeneratorControlをPublishすることで生成の停止/再開をコントロールする
        _subscriber.Subscribe(control => 
        {
            _isGeneratable = control.IsGeneratable;
        }).AddTo(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int r = UnityEngine.Random.Range(0, _prefabs.Length);
            _lastInstantiatedPrefab.Value = Instantiate(_prefabs[r]/*, _parent*/);
        }
    }

    public async UniTask GenerateRegularlyAsync(CancellationTokenSource tokenSource)
    {
        _cts = tokenSource;

        try
        {
            while (true)
            {
                // インターバル中に生成可能フラグが立っても次の生成タイミングが来るまでは生成されない。
                if (_isGeneratable)
                {
                    int r = UnityEngine.Random.Range(0, _prefabs.Length);
                    // 生成した際の初期化処理を別のコンポーネントに委任する
                    _lastInstantiatedPrefab.Value = Instantiate(_prefabs[r]/*, _parent*/);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: tokenSource.Token);
            }
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("生成処理がキャンセルされました。: " + e.Message);
        }
    }

    public void GenerateRegularlyCancel() => _cts?.Cancel();

    void OnDestroy()
    {
        _cts?.Cancel();
    }
}
