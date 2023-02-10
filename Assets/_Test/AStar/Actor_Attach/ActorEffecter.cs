using UnityEngine;

/// <summary>
/// キャラクターの周囲のオブジェクトを操作するコンポーネント
/// </summary>
public class ActorEffecter : MonoBehaviour
{
    // 周囲の操作可能なオブジェクトの数に応じて増やす
    readonly int ResultsLength = 4;

    [Header("操作可能な範囲")]
    [SerializeField] float _effectRange = 5;
    [Header("視界に映るオブジェクトのレイヤー")]
    [SerializeField] LayerMask _effectLayer = 8;

    Collider[] _results;

    void Awake()
    {
        _results = new Collider[ResultsLength];
    }

    internal void EffectAround()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _effectRange, _results, _effectLayer);

        // TODO:仮の処理として取得した対象を破棄する処理、実際はインターフェースを経由する
        Destroy(_results[0].gameObject);
        Debug.Log("操作");
    }
}
