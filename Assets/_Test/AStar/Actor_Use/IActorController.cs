using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V���ő��삷�邽�߂Ɏg���A�L�����N�^�[�̍s������������C���^�[�t�F�[�X
/// </summary>
public interface IActorController
{
    public bool IsTransitionIdleState();

    public void MoveToTarget(bool isDash);
    public bool IsTransitionToWanderStateFromMoveState();
    public bool IsTransitionToAnimationStateFromMoveState();
    public void MoveCancel();

    public bool IsTransitionAnimationState();
    public void PlayAnim();

    public void PlayLookAround();
    public bool IsTransitionToMoveStateFromWanderStateAfterLookAroundDOtweenAnimation();

    public bool IsMovaStateAndWanderStateAndAnimationStateIsCancelToStateDeadState();

    public void PlayDiscoverAnim();
    public bool IsTransitionToMoveStateFromDiscoverState();

    public void FromAnyStateDead();
}
