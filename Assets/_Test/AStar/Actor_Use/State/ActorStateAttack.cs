using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �U������X�e�[�g�̃N���X
/// </summary>
internal class ActorStateAttack : ActorStateBase
{
    internal ActorStateAttack(IStateControl movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.PlayAttackAnim();
    }
}
