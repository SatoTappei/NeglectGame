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
    [Header("参照したい処理を持つコンポーネントへの参照")]
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] ActorStatusUIManager _actorStatusUIManager;

    void Awake()
    {
        // Awake()とEnabled()の後、Start()の前に呼ばれる
        _generator.LastInstantiatedPrefab.Where(instance => instance != null).Subscribe(instance =>
        {
            // Waypointを生成した後にGeneratorコンポーネント生成処理をしないといけない
            // 時間的結合をしているので呼び出し順に注意
            List<Vector3> list = _waypointManager.GetWaypointListWithWaypointType(WaypointType.Exit);
            int r = Random.Range(0, list.Count);
            instance.transform.position = list[r];

            // UIに情報を引き渡す
            // 表示させるUIの取得
            // アイコン,体力の初期値をセット
            // 体力が変動するたびにUIを更新
            // 特定のタイミングで台詞

            ActorStatusUI actorStatusUI = _actorStatusUIManager.GetUnUsedUI();

            //Sprite icon = actorRxMediator.ActorStatusSO.Icon;
            //int hp = actorRxMediator.CurrentHp.Value;
            //actorStatusUI.Init(icon, hp);
            ActorStatusSO status = instance.GetComponent<Actor>().ActorStatus;
            actorStatusUI.Init(status.Icon, status.MaxHp);

            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            currentHp.Subscribe(i => actorStatusUI.SetHp(i)).AddTo(instance);

            //actorRxMediator.CurrentHp.Subscribe(i => actorStatusUI.SetHp(i)).AddTo(instance);
        });
    }
}
