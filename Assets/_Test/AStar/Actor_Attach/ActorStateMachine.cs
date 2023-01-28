using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A*を用いたキャラクターのステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    internal enum StateID
    {
        Idle,
        Move,
        Wander,
        Anim,
        Discover,
        Dead
    }

    // 各ステートはこのインターフェースで実装されているメソッドを適切なタイミングで呼び出す
    IActorController _actorController;
    ActorStateBase _currentState;
    Dictionary<StateID, ActorStateBase> _stateDic;

    void Awake()
    {
        // 動的にステートを追加しないので初期容量を超えることはない
        int capacity = Enum.GetValues(typeof(StateID)).Length;
        _stateDic = new Dictionary<StateID, ActorStateBase>(capacity);
    }

    void Start()
    {
        _actorController = GetComponent<IActorController>();

        // TOOD:ここら辺の生成処理はVContainerに任せられないか
        ActorStateIdle idle = new ActorStateIdle(_actorController, this);
        ActorStateMove move = new ActorStateMove(_actorController, this);
        ActorStateWander wander = new ActorStateWander(_actorController, this);
        ActorStateAnimation anim = new ActorStateAnimation(_actorController, this);
        ActorStateDiscover discover = new ActorStateDiscover(_actorController, this);
        ActorStateDead dead      = new ActorStateDead(_actorController, this);

        _stateDic.Add(StateID.Idle, idle);
        _stateDic.Add(StateID.Move, move);
        _stateDic.Add(StateID.Wander, wander);
        _stateDic.Add(StateID.Anim, anim);
        _stateDic.Add(StateID.Discover, discover);
        _stateDic.Add(StateID.Dead, dead);

        // 登場時にアニメーションを再生するため
        _currentState = anim;

        /*
         *  超重要:歩き、うろうろ、アニメーション中にキャンセル処理
         *         Qキーを押したらDeadステートに遷移するようにする
         */
        // Wキーで発見処理に遷移するようにした
        // 

        // idle => move
        // idle => wander
        // idle => anim

        // move => anim
        // move => wander 出来た
        // move => idle

        // wander => move 出来た
        // wander => anim
        // wander => idle

        // anim => idle
        // anim => move 出来た
        // anim => dead
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBase GetNextState(StateID stateID)
    {
        if (_stateDic.TryGetValue(stateID, out ActorStateBase state))
        {
            return state;
        }
        else
        {
            Debug.LogError("遷移先のステートが辞書内にありません: " + stateID);
            return null;
        }
    }
}
