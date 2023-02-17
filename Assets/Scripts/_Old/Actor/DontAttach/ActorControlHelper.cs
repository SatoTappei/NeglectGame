using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター制御で使用する計算/判定系のメソッドを抜き出したヘルパークラス
/// </summary>
internal class ActorControlHelper
{
    internal string StateIDToString(StateID id)
    {
        switch (id)
        {
            case StateID.Appear: return "Appear";
            case StateID.Attack: return "Attack";
            case StateID.Joy: return "Joy";
            case StateID.LookAround: return "LookAround";
            case StateID.Panic: return "Panic";
        }

        Debug.LogError("ステートIDが登録されていません:" + id);
        return string.Empty;
    }
}
