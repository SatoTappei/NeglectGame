using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generatorで生成したインスタンスを参照したいコンポーネントの処理を行う
/// このコンポーネントによって初期化されなくてもゲーム自体の動作はする
/// </summary>
public class GenerateDecorator : MonoBehaviour
{
    /// <summary>Awake()とOnEnable()より後、Start()の前に呼ばれる</summary>
    public void Decorate(GameObject instance)
    {
        // 位置を階段にする
        // UIに情報を引き渡す
        Debug.Log("でこれーしょｎ");
    }
}
