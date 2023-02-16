/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����A�e�X�e�[�g�̍s�����J�ڏ�������������C���^�[�t�F�[�X
/// </summary>
internal interface IStateControl
{
    // TODO:���\�b�h�����悭�Ȃ�
    //      �J�ڐ�̃X�e�[�g���Ƃ��̃X�e�[�g����ʏ�J�ڂ���X�e�[�g��������
    void PlayAnim(StateID current, StateID next);

    void MoveToRandomWaypoint();
    void RunToTarget();
    void MoveToExit();
    void CancelMoving();

    bool IsTransitionable();
    bool IsEqualNextState(StateID state);
    bool IsDead();
    bool IsSightTarget();
    bool IsCompleted();

    // �v���C���[�̎��͂̃I�u�W�F�N�g�̑����������
    void EffectAroundEffectableObject();
}