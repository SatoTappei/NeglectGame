using UniRx;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generatorで生成したオブジェクトを生成したタイミングで
/// 参照したいコンポーネントを使って初期化を肩代わりする
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;
    //[Header("参照したい処理を持つコンポーネントへの参照")]
    [SerializeField] WaypointManager _waypointManager;

    void Awake()
    {
        // Awake()とEnabled()の後、Start()の前に呼ばれる
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject != null).Subscribe(gameObject =>
        {
            // Waypointを生成した後にGeneratorコンポーネント生成処理をしないといけない
            // 時間的結合をしているので呼び出し順に注意
            List<Vector3> list = _waypointManager.GetWaypointListWithWaypointType(WaypointType.Exit);
            int r = Random.Range(0, list.Count);
            gameObject.transform.position = list[r];

            // UIに情報を引き渡す
        });
    }
}
