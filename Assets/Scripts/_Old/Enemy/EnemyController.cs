using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を制御するコンポーネント
/// </summary>
public class EnemyController : MonoBehaviour
{
    /* 
     *  仮状態、本来はステートマシンにするべき 
     */

    [SerializeField] GameObject _weapon;

    // 取りうる状態は3つ
    // 待機…生成時にはこの状態
    // 攻撃…一定周期で攻撃を行う
    // 死亡…一定時間後に復活する(登場演出を行って待機に戻る)

    void Start()
    {
        // 一時的な処理として一定間隔で武器の表示/非表示を切り替えるを繰り返す
        // 仮のダメージを与える処理
        InvokeRepeating(nameof(ActiveWeapon), 0, 1);
    }

    void ActiveWeapon()
    {
        _weapon.SetActive(!_weapon.activeInHierarchy);
    }
}
