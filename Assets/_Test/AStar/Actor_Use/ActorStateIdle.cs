using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateID = ActorStateMachine.StateID;

/// <summary>
/// ���̏�őҋ@����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateIdle : ActorStateBase
{
    internal ActorStateIdle(IActorController movable, ActorStateMachine stateMachine)
        : base(movable, stateMachine) { }
}
