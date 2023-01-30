using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �^�[�Q�b�g�Ɍ������Ĉړ�����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    internal ActorStateMove(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.MoveToTarget();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
        }
        else if (_actorController.IsTransitionToPanicState())
        {
            ChangeState(StateID.Panic);
        }
        else if (_actorController.IsTransitionable())
        {
            ChangeState(StateID.Wander);
        }
    }

    protected override void Exit()
    {
        _actorController.CancelMoveToTarget();
    }
}