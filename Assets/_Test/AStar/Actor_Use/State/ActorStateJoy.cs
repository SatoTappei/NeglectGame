using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �ڕW��B�������ۂ̊�т̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateJoy : ActorStateBase
{
    internal ActorStateJoy(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAppearAnim();
    }
}
