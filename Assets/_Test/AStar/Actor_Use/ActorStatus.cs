using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの各種ステータスのデータのみを持つクラス
/// </summary>
internal class ActorStatus
{
    int _hp;
    GameObject _treasure;

    public int Hp { get => _hp; set => _hp = value; }
    public GameObject Treasure { get => _treasure; set => _treasure = value; }

    /* 
    *  今までは各種動作をするコンポーネントが対応する内部の値を保持していた
    *  このクラスにプレイヤーのステータスなどを保持しておき
    *  他から取り出せるようにするのがベスト？
    *  デメリットとしてはクラス間の結合が強くなってしまう
    */
}
