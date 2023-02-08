using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// ゲーム全体の流れを管理するコンポーネント(テスト用)
/// </summary>
public class TestInGameStream : MonoBehaviour
{
    /* 
     *  ステージの自動生成をしない状態できちんと動くように作る 
     */

    [SerializeField] ActorGenerator _actorGenerator;

    void Start()
    {
        //await UniTask.WaitUntil(()=>Input.GetKeyDown(KeyCode.Space));
        // 演出を待つために1フレームawaitする
        Hoge(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid Hoge(CancellationToken token)
    {
        await UniTask.Yield();
        await _actorGenerator.GenerateRegularly(token);
    }
}
