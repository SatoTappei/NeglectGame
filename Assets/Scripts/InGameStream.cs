using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ゲーム全体の流れを制御するコンポーネント
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] DungeonBuilder _dungeonBuilder;
    [SerializeField] InGameTimer _inGameTimer;
    [SerializeField] Generator _generator;

    async void Start()
    {
        // カメラはクォータービューで固定

        // タイトル画面
        // ボタンをクリックでスタート
        // ステージ生成演出
        // フェードしてボタンとロゴが消える
        // 演出終了後にタイマースタート
        // n秒間隔で冒険者がダンジョンにやってくる
        //  階段の位置に冒険者を生成
        //  冒険者はダンジョンをうろうろする
        // プレイヤーは3種類の罠のうちどれかを選んで好きなところに罠を置ける
        // 罠は冒険者にダメージを与える
        // n秒経ったらゲームオーバー

        // リザルト
        // 何人の冒険者を葬ったか

        // タイトルに戻る

        // 必要なUI
        // 左上:タイマー
        // 右上:葬った冒険者の数(スコア)
        // 右下:罠用のボタン3つ
        // 右:各冒険者のステータスアイコン5つ

        // ダンジョン生成時にアニメーションさせるのでCapacityを増やして警告を消す
        // 処理負荷が問題になった場合はアニメーションをやめること
        DOTween.SetTweensCapacity(500, 50);

        // キャラクターを生成するのにダンジョンの地形情報が必要なので
        // 先にダンジョンを生成する必要がある。
        _dungeonBuilder.Build();

        // ここで1フレーム待たないとRayが正常に判定しない
        await UniTask.Yield();

        // キャラクター生成時にはグリッドの情報が必要なので先に生成する必要がある
        _pathfindingGrid.GenerateGrid();

        // DungeonBuilderで生成したWaypointを取得して経路探索に使えるようにする
        _waypointManager.RegisterWaypoint();

        // インゲームのタイマーと冒険者の生成はかみ合っていない
        // インゲームのタイマーのスタートと同時に敵の生成を行うGeneratorも起動する
        // Generatorは独自の間隔で生成している
        _generator.GenerateRegularlyAsync(new CancellationTokenSource()).Forget();

        // インゲームのタイマーの開始はメソッドの呼び出しで行うが
        // 値の加算はMessagePipeを用いたメッセージングで行う
        await _inGameTimer.StartAsync(this.GetCancellationTokenOnDestroy());

    }
}
