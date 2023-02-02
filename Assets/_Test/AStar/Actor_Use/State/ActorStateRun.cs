using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �^�[�Q�b�g�Ɍ������đ����Ĉړ�����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateRun : ActorStateMove
{
    internal ActorStateRun(IStateControl movable, ActorStateMachine stateMachine)
    : base(movable, stateMachine) { }

    protected override void Enter()
    {
        _stateControl.RunToTarget();
    }

    protected override void Exit()
    {
        _stateControl.RunEndable();
    }
}
