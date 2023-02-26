using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターによる操作を行える処理を実装するインターフェース
/// </summary>
public interface IEffectable
{
    void Effect(string message);
}
