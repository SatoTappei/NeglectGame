using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターが行う行動を実装するインターフェース
/// </summary>
public interface IActorController
{
    public void MoveStart();
    public void MoveCancel();
    public void PlayAnim();
}
