using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索を使用した移動の制御の処理を宣言したインターフェース
/// </summary>
public interface IMovable
{
    public void MoveStart(Vector3 targetPos);
}
