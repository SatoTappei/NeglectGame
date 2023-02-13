using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// 冒険者を生成するコンポーネント
/// </summary>
public class ActorGenerator : MonoBehaviour
{
    // 最大3人以上はフィールドに生成させない
    // 冒険者が生成されるとステータスを表すUIがシュッと横から出てくる
    [SerializeField] GameObject _prefab;

    // テスト用の生成可能フラグ
    // 一通り作り終わったらフラグは別の場所に管理するようにする
    bool _isGeneratable;

    internal async UniTask GenerateRegularly(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        while (true)
        {
            GenerateActor();
            // テスト用にGキーを押したら生成するようにしている
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.G), cancellationToken: token);
        }
    }

    void GenerateActor()
    {
        Instantiate(_prefab, transform.position, Quaternion.identity);
        // ここに生成時の演出を書く
    }
}
