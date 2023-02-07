using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    async void Start()
    {
        // 事前に演出を挟みたいのでSpaceが押されるまでawait
        await UniTask.WaitUntil(()=>Input.GetKeyDown(KeyCode.Space));
        await _actorGenerator.GenerateRegularly(this.GetCancellationTokenOnDestroy());
    }
}
