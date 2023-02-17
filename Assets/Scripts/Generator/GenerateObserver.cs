using UniRx;
using UnityEngine;

/// <summary>
/// Generatorで生成したオブジェクトを生成したタイミングで
/// 参照したいコンポーネントを使って初期化を肩代わりする
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;
    //[Header("参照したい処理を持つコンポーネントへの参照")]

    void Awake()
    {
        // Awake()とEnabled()の後、Start()の前に呼ばれる
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject != null).Subscribe(gameObject =>
        {
            // 位置を階段にする

            // UIに情報を引き渡す
        });
    }
}
