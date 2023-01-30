using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�Ɍ������đ����Ĉړ�����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateRun : ActorStateMove
{
    internal ActorStateRun(IActorController movable, ActorStateMachine stateMachine)
    : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _actorController.RunToTarget();
    }
}
