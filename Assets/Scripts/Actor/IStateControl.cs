/// <summary>
/// �X�e�[�g�}�V�������e�X�e�[�g�̑J�ڂ�����̂ɕK�v�ȏ�������������C���^�[�t�F�[�X
/// �e�X�e�[�g�͂��̃C���^�[�t�F�[�X�Ŏ�������鏈����g�ݍ��킹�ăX�e�[�g���̏���������Ă���
/// </summary>
public interface IStateControl
{
    void PlayAnimation(string name);
    void MoveToWaypoint();
    void MoveToInSightObject();

    float GetAnimationClipLength(string name);
    /// <summary>
    /// ���E���̃I�u�W�F�N�g(InSightObject)�Ɉړ�����ۂɂ��g���̂Ń��\�b�h����ύX����
    /// </summary>
    /// <returns></returns>
    bool IsArrivalWaypoint();
    SightableObject GetInSightObject();
}
