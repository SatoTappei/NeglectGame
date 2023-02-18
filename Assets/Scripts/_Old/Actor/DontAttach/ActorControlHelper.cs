using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター制御で使用する計算/判定系のメソッドを抜き出したヘルパークラス
/// </summary>
internal class ActorControlHelper
{
    internal string StateIDToString(StateIDOld id)
    {
        switch (id)
        {
            case StateIDOld.Appear: return "Appear";
            case StateIDOld.Attack: return "Attack";
            case StateIDOld.Joy: return "Joy";
            case StateIDOld.LookAround: return "LookAround";
            case StateIDOld.Panic: return "Panic";
        }

        Debug.LogError("ステートIDが登録されていません:" + id);
        return string.Empty;
    }
}
