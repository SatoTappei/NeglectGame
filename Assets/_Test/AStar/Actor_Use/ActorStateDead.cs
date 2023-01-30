using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// ��������ȏ㓮�����Ȃ���Ԃ̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStateDead : ActorStateBase
{
    internal ActorStateDead(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.PlayDeadAnim();
    }
}