using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンで操作するために使う、キャラクターの行動を実装するインターフェース
/// </summary>
public interface IActorController
{
    public void MoveToTarget();
    public void MoveCancel();
    public void PlayAnim();
}
