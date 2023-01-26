using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*を用いたキャラクターのステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    // 各ステートはこのインターフェースで実装されている
    // メソッドを適切なタイミングで呼び出す
    IActorController _movable;

    void Start()
    {
        _movable = GetComponent<IActorController>();

        ActorStateMove actorStateMove = new ActorStateMove(_movable);
        actorStateMove.Update();
    }
}
