using UnityEngine;

/// <summary>
/// キャラクターの周囲のオブジェクトを操作するコンポーネント
/// </summary>
public class ActorEffecter : MonoBehaviour
{
    /// <summary>周囲の操作可能なオブジェクトの数に応じて増やす</summary>
    static readonly int ResultsLength = 4;

    [Header("操作可能な範囲")]
    [SerializeField] float _effectRadius = 5;
    [Header("視界に映るオブジェクトのレイヤー")]
    [SerializeField] LayerMask _effectLayer = 8;

    Collider[] _results = new Collider[ResultsLength];

    internal void EffectAround()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _effectRadius, _results, _effectLayer);

        foreach (Collider collider in _results)
        {
            if (collider == null) break;
            //collider.gameObject.GetComponent<IEffectable>().EffectByActor();
        }
    }
}
