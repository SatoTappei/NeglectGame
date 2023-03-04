using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 冒険者を生成するジェネレータの制御をテストするコンポーネント
/// </summary>
public class GeneratorTestControl : MonoBehaviour
{
    [Inject]
    readonly IPublisher<GeneratorControl> _publisher;

    [SerializeField] Generator _generator;

    void Start()
    {
        // ジェネレータの起動はコンポーネントの参照してメソッド呼び出しで行うが
        // 生成の停止/再開はメッセージを介して行う
        _generator.GenerateRegularlyAsync(new CancellationTokenSource()).Forget();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: false));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: true));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //_generator.GenerateRegularlyCancel();
        }
    }
}
