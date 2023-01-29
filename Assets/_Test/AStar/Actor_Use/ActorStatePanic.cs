using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �����������̃X�e�[�g�̃N���X
/// </summary>
internal class ActorStatePanic : ActorStateBase
{
    internal ActorStatePanic(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        Debug.Log("�����`");
        _actorController.PlayPanicAnim();
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionable())
        {
            ChangeState(StateID.Run);
            return;
        }
        base.Stay();
    }

    protected override void Exit()
    {
        base.Exit();
    }
}