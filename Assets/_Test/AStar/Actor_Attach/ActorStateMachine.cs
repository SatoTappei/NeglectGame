using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

internal enum StateID
{
    Non,
    Appear,
    Move,
    Run,
    Attack,
    Joy,
    LookAround,
    Panic,
    Escape,
    Dead
}

/// <summary>
/// A*を用いたキャラクターのステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    ActorStateBase _currentState;
    Dictionary<StateID, ActorStateBase> _stateDic;
    IStateControl _stateControl;

    // 各ステートはインターフェースで実装されているメソッドを適切なタイミングで呼び出す
    internal IStateControl StateControl { get => _stateControl; }

    void Awake()
    {
        // 動的にステートを追加しないので初期容量を超えることはない
        int capacity = Enum.GetValues(typeof(StateID)).Length;
        _stateDic = new Dictionary<StateID, ActorStateBase>(capacity);
    }

    void Start()
    {
        _stateControl = GetComponent<IStateControl>();

        /* 
         *  そもそも各ステートはIStateControlを知っている必要はない
         *  このコンポーネントが仲介役として各ステートのメソッドを管理する
         */

        // TOOD:ここら辺の生成処理はVContainerに任せられないか
        ActorStateAppear appear          = new ActorStateAppear(this);
        ActorStateMove move              = new ActorStateMove(this);
        ActorStateRun run                = new ActorStateRun(this);
        ActorStateAttack attack          = new ActorStateAttack(this);
        ActorStateJoy joy                = new ActorStateJoy(this);
        ActorStateLookAround lookAround  = new ActorStateLookAround(this);
        ActorStatePanic panic            = new ActorStatePanic(this);
        ActorStateEscape escape          = new ActorStateEscape(this);
        ActorStateDead dead              = new ActorStateDead(this);

        // 遷移先には選ばれないのでStateID.Appearの追加処理はしないで良い
        _stateDic.Add(StateID.Move, move);
        _stateDic.Add(StateID.Run, run);
        _stateDic.Add(StateID.Attack, attack);
        _stateDic.Add(StateID.Joy, joy);
        _stateDic.Add(StateID.LookAround, lookAround);
        _stateDic.Add(StateID.Panic, panic);
        _stateDic.Add(StateID.Escape, escape);
        _stateDic.Add(StateID.Dead, dead);

        // 登場時にアニメーションを再生するため
        _currentState = appear;
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBase GetState(StateID stateID)
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
