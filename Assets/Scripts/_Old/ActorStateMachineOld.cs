using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateIDOld
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
public class ActorStateMachineOld : MonoBehaviour
{
    ActorStateBaseOld _currentState;
    Dictionary<StateIDOld, ActorStateBaseOld> _stateDic;
    IStateControl _stateControl;

    // 各ステートはインターフェースで実装されているメソッドを適切なタイミングで呼び出す
    internal IStateControl StateControl { get => _stateControl; }

    void Awake()
    {
        // 動的にステートを追加しないので初期容量を超えることはない
        int capacity = Enum.GetValues(typeof(StateIDOld)).Length;
        _stateDic = new Dictionary<StateIDOld, ActorStateBaseOld>(capacity);
    }

    void Start()
    {
        _stateControl = GetComponent<IStateControl>();

        // TOOD:ここら辺の生成処理はVContainerに任せられないか
        ActorStateAppear appear          = new ActorStateAppear(this);
        ActorStateMoveOld move              = new ActorStateMoveOld(this);
        ActorStateRun run                = new ActorStateRun(this);
        ActorStateAttack attack          = new ActorStateAttack(this);
        ActorStateJoy joy                = new ActorStateJoy(this);
        ActorStateLookAround lookAround  = new ActorStateLookAround(this);
        ActorStatePanic panic            = new ActorStatePanic(this);
        ActorStateEscape escape          = new ActorStateEscape(this);
        ActorStateDeadOld dead              = new ActorStateDeadOld(this);

        // 遷移先には選ばれないのでStateID.Appearの追加処理はしないで良い
        _stateDic.Add(StateIDOld.Move, move);
        _stateDic.Add(StateIDOld.Run, run);
        _stateDic.Add(StateIDOld.Attack, attack);
        _stateDic.Add(StateIDOld.Joy, joy);
        _stateDic.Add(StateIDOld.LookAround, lookAround);
        _stateDic.Add(StateIDOld.Panic, panic);
        _stateDic.Add(StateIDOld.Escape, escape);
        _stateDic.Add(StateIDOld.Dead, dead);

        // 登場時にアニメーションを再生するため
        _currentState = appear;
    }

    void Update()
    {
        _currentState = _currentState.Update();
    }

    internal ActorStateBaseOld GetState(StateIDOld stateID)
    {
        if (_stateDic.TryGetValue(stateID, out ActorStateBaseOld state))
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
