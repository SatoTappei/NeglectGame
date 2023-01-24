using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*を用いたキャラクターのステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    [SerializeField] PathfindingPresenter _pathfindingPresenter;

    void Start()
    {
        // 意欲が無くなった場合はダンジョンから脱出する
        // 目的を達成した場合もダンジョンから脱出する
        //  目的 = 目的の部屋に到達すること
        //  ボス部屋に到達、お宝部屋に到達、うろうろしただけで帰る
        // 影響マップに従って行動する

        // ダンジョンにやってくる
        // 一直線で目的の部屋に向かうのはおかしい <= ダンジョンでうろうろさせる
        // うろうろ…ダンジョンのランダムな箇所に向かうようにする
        //           到着したら次のランダムな箇所に向かう
        // うろうろ中二目的の部屋を見つけたらその部屋に入っていく


    }

    void Update()
    {
        
    }
}
