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

            // 現在はActorにSOを持たせているが、ActorStatusとの仲介役を作ってそっちに持たせる
            // そうすることでActorのSOへのプロパティを消す
            // ManagerとUIどっちも操作しているのであんま良くない？

            // こっちはキャラクター側からの取得、UI側はどこから来たか知らなくてよい
            ActorStatusSO status = instance.GetComponent<Actor>().ActorStatus;
            // こっちはUI側への値を渡す、UI側はどこから来たか知らなくてよい
            ActorStatusUI statusUI = _actorStatusUIManager.GetNewActiveUI(status.Icon, status.MaxHp);

            var state = instance.GetComponent<ActorStateMachine>().CurrentState;

            // HPControlの値をUIに割り当てる、これもお互いを知らなくてよい
            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            System.IDisposable disposable = currentHp.Subscribe(i => statusUI.SetHp(i)).AddTo(instance);

            state.Where(s => s.Type == StateType.Goal || s.Type == StateType.Dead).Subscribe(_ => 
            {
                disposable.Dispose();
                statusUI.Release();
            }).AddTo(instance);

            //currentHp.Where(i => i <= 0).Skip(1).Subscribe(_ => statusUI.Release()).AddTo(instance);

            // 現在は↑のサブスクが解除されていないのでおかしな挙動になる
            // どうにかキャラ側の任意のタイミングで処理を消せるようにする
        });
    }
}
