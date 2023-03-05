using UniRx;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] PauseControl _pauseControl;

    void Awake()
    {
        // Awake()とEnabled()の後、Start()の前に呼ばれる
        _generator.LastInstantiatedPrefab.Where(instance => instance != null).Subscribe(instance =>
        {
            // Waypointを生成した後にGeneratorコンポーネント生成処理をしないといけない
            // 時間的結合をしているので呼び出し順に注意
            _waypointManager.SetRandomWaypoint(instance, WaypointType.Exit);

            ActorStatusHolder statusHolder = instance.GetComponent<ActorStatusHolder>();
            ActorStatusUI statusUI = _actorStatusUIManager.GetNewActiveUI(statusHolder.Icon, statusHolder.MaxHp);

            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            System.IDisposable disposable = currentHp.Subscribe(i => 
            {
                statusUI.SetHp(i, statusHolder.MaxHp);
            }).AddTo(instance);

            var currentState = instance.GetComponent<ActorStateMachine>().CurrentState;
            currentState.Subscribe(state => 
            {
                string line = statusHolder.GetLineWithState(state.Type);
                statusUI.PrintLine(line);
            }).AddTo(instance);
            currentState.Where(state => state.Type == StateType.Goal || state.Type == StateType.Dead).Subscribe(state =>
            {
                disposable.Dispose();
                _actorMonitor.DetectGoalOrDeadState(state.Type);

                // 台詞表示の時間を確保するため遅延させる
                DOVirtual.DelayedCall(1.5f, () => 
                {
                    _generateControl.CountDown();
                    statusUI.Release();
                    _pauseControl.Remove(instance);
                }).SetLink(instance);
            }).AddTo(instance);

            _generateControl.CountUp();
            _pauseControl.Add(instance);
        });
    }
}
