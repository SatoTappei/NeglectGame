using UniRx;
using UnityEngine;

/// <summary>
/// Generatorで生成したオブジェクトを生成したタイミングで
/// 参照したいコンポーネントを使って初期化を肩代わりする
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;

    void Awake()
    {
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject!=null).Subscribe(gameObject => 
        {
            Debug.Log(gameObject.name);
        });
    }

    /// <summary>Awake()とOnEnable()より後、Start()の前に呼ばれる</summary>
    public void Decorate(GameObject instance)
    {
        // 位置を階段にする
        // UIに情報を引き渡す
    }
}
