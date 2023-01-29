using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// �A�j���[�V�����̍Đ����s���X�e�[�g�̃N���X
/// </summary>
internal class ActorStateAnimation : ActorStateBase
{
    internal ActorStateAnimation(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.PlayAppearAnim();
        Debug.Log("�A�j���Đ�");
        base.Enter();
    }

    protected override void Stay()
    {
        if (_actorController.IsTransitionToDeadState())
        {
            ChangeState(StateID.Dead);
            return;
        }

        if (_actorController.IsTransitionable())
        {
            ChangeState(StateID.Move);
            return;
        }

        Debug.Log("�ʏ�");
        base.Stay();
    }

    protected override void Exit()
    {
        Debug.Log("Exit����");
        base.Exit();
    }
}
