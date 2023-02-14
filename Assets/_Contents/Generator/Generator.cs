using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using MessagePipe;
using VContainer;

// 冒険者はn人以上生成させない
// 定期的にフィールド内の冒険者数をチェックする
// 今何人いるかのフラグが必要
// 生成/脱出/死亡時に上下する

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
    [Header("生成する間隔")]
    [SerializeField] float _interval = 1.0f;

    bool _isGeneratable = true;
    /// <summary>
    /// 外部からキャンセル用のメソッドを呼ぶだけで生成処理をキャンセルできるように
    /// CancellationTokenSourceをこちら側で保持しておく
    /// </summary>
    CancellationTokenSource _tokenSource;

    void Start()
    {
        // 外部からGeneratorControlをPublishすることで生成の停止/再開をコントロールする
        _subscriber.Subscribe(control => 
        {
            _isGeneratable = control.IsGeneratable;
        }).AddTo(this);
    }

    public async UniTask GenerateRegularlyAsync(CancellationTokenSource tokenSource)
    {
        _tokenSource = tokenSource;

        try
        {
            while (true)
            {
                // インターバル中に生成可能フラグが立っても次の生成タイミングが来るまでは生成されない。
                if (_isGeneratable)
                {
                    int r = UnityEngine.Random.Range(0, _prefabs.Length);

                    // TODO:生成された際に初期化したい
                    // 生成する場所を階段にしたい
                    // 生成したらUIをシュッと出す演出をしたい
                    Instantiate(_prefabs[r]);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: tokenSource.Token);
            }
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("生成処理がキャンセルされました。: " + e.Message);
        }
    }

    public void GenerateRegularlyCancel() => _tokenSource?.Cancel();

    void OnDestroy()
    {
        _tokenSource.Cancel();
    }
}
