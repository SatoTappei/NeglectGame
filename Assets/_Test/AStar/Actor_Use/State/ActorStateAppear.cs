using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �o�ꎞ�̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateAppear : ActorStateBase
{
    internal ActorStateAppear(IStateControl movable, ActorStateMachine stateMachine)
            : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAppearAnim();
    }

    protected override void Stay()
    {
        if (_stateControl.IsTransitionable())
        {
            ChangeState(StateID.Move);
        }
    }
}