using UnityEngine;

/// <summary>
/// �X�e�[�g�}�V�������e�X�e�[�g�̑J�ڂ�����̂ɕK�v�ȏ�������������C���^�[�t�F�[�X
/// �e�X�e�[�g�͂��̃C���^�[�t�F�[�X�Ŏ�������鏈����g�ݍ��킹�ăX�e�[�g���̏���������Ă���
/// </summary>
public interface IStateControl
{
    void PlayAnimation(string name);
    void MoveToWaypoint();
    void MoveTo(Vector3 targetPos);
    void ToggleSight(bool isActive);
    float GetAnimationClipLength(string name);
    bool IsArrivalTargetPos();
    SightableObject GetInSightObject();
}
