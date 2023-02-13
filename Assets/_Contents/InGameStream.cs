using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ゲーム全体の流れを制御するコンポーネント
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] DungeonBuilder _dungeonBuilder;

    void Start()
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
        _dungeonBuilder.Build();


    }

    void Update()
    {
        
    }
}
