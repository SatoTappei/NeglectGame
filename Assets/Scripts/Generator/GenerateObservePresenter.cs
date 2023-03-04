using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// Generatorで生成したオブジェクトを生成したタイミングで
/// 参照したいコンポーネントを使って初期化を肩代わりする
/// </summary>
public class GenerateObservePresenter : MonoBehaviour
{
    [SerializeField] Generator _generator;
    [Header("参照したい処理を持つコンポーネントへの参照")]
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] ActorStatusUIManager _actorStatusUIManager;
    [SerializeField] ActorMonitor _actorMonitor;
    [SerializeField] GenerateControl _generateControl;

    void Awake()
    {
        // Awake()とEnabled()の後、Start()の前に呼ばれる
        _generator.LastInstantiatedPrefab.Where(instance => instance != null).Subscribe(instance =>
        {
            // Waypointを生成した後にGeneratorコンポーネント生成処理をしないといけない
            // 時間的結合をしているので呼び出し順に注意
            //List<Vector3> list = _waypointManager.GetWaypointListWithWaypointType(WaypointType.Exit);
            //int r = Random.Range(0, list.Count);
            //instance.transform.position = list[r];

            _waypointManager.SetRandomWaypoint(instance, WaypointType.Exit);

            // TODO:怒涛の参照取得＆規定数を超えてUIを表示させようとするとエラー
            ActorStatusHolder statusHolder = instance.GetComponent<ActorStatusHolder>();
            ActorStatusUI statusUI = _actorStatusUIManager.GetNewActiveUI(statusHolder.Icon, statusHolder.MaxHp);

            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            System.IDisposable disposable = currentHp.Subscribe(i => statusUI.SetHp(i)).AddTo(instance);

            var currentState = instance.GetComponent<ActorStateMachine>().CurrentState;
            currentState.Where(state => state.Type == StateType.Goal || state.Type == StateType.Dead)
                .Subscribe(state =>
                {
                    disposable.Dispose();
                    statusUI.Release();

                    _actorMonitor.DetectGoalOrDeadState(state.Type);
                    _generateControl.Remove();

                }).AddTo(instance);

           _generateControl.Add();
        });
    }
}
