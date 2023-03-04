using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ゲーム全体の流れを制御するコンポーネント
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] TitleUIControl _titleUIControl;
    [SerializeField] ResultUIControl _resultUIControl;
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] DungeonBuilder _dungeonBuilder;
    [SerializeField] TrapManager _trapManager;
    [SerializeField] InGameTimer _inGameTimer;
    [SerializeField] Generator _generator;

    void Start()
    {
        Stream(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid Stream(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // ダンジョン生成時にアニメーションさせるのでCapacityを増やして警告を消す
        // 処理負荷が問題になった場合はアニメーションをやめること
        DOTween.SetTweensCapacity(500, 50);

        // タイトル画面から遷移するのを待つ
        await _titleUIControl.TitleStateAsync(token);

        AudioManager.Instance.PlayBGM("BGM_インゲーム");

        // キャラクターを生成するのにダンジョンの地形情報が必要なので
        // 先にダンジョンを生成する必要がある。
        _dungeonBuilder.Build();

        // ここでダンジョン生成時のアニメーション分待たないとRayが反応しない
        // ↓ゲームスタート！の演出を作る
        await UniTask.Delay(System.TimeSpan.FromSeconds(1.0f));

        // キャラクター生成時にはグリッドの情報が必要なので先に生成する必要がある
        _pathfindingGrid.GenerateGrid();

        // DungeonBuilderで生成したWaypointを取得して経路探索に使えるようにする
        _waypointManager.RegisterWaypoint();

        // インゲームのタイマーと冒険者の生成はかみ合っていない
        // インゲームのタイマーのスタートと同時に敵の生成を行うGeneratorも起動する
        // Generatorは独自の間隔で生成している
        CancellationTokenSource cts = new();
        _generator.GenerateRegularlyAsync(cts).Forget();

        _trapManager.Init();

        // インゲームのタイマーの開始はメソッドの呼び出しで行うが
        // 値の加算はMessagePipeを用いたメッセージングで行う
        await _inGameTimer.StartAsync(token);
        cts.Cancel();

        await _resultUIControl.AnimationAsync(token);
    }
}
