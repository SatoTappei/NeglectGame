/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����A�e�X�e�[�g�̍s�����J�ڏ�������������C���^�[�t�F�[�X
/// </summary>
internal interface IStateControl
{
    // Controller���ɃC���^�[�t�F�[�X����������Ă���̂�
    // ���̃��\�b�h�Q��Controller�̋@�\���g�킹�Ă�����Ƃ��������ɂȂ�

    // TODO:���\�b�h�����悭�Ȃ�
    void PlayAnim(StateID current, StateID next);
    void CancelAnim(string name);

    void MoveToTarget();
    void RunToTarget();
    void MoveToExit();
    void CancelMoveToTarget();

    // �A�j���[�V�������I������^�C�~���O�Ŏ��̃X�e�[�g�������Ƃ���邩�炢��Ȃ���������Ȃ�
    bool IsTransitionable();
    bool IsEqualNextState(StateID state);
    bool IsDead();

    // �U���X�e�[�g�ł����g���Ă��Ȃ��A����Ă���G�����񂾂����肷�鏈��
    bool IsTargetLost();

    //// �S�ẴX�e�[�g�͊�{���̑J�ڏ����ɏ]��
    //public bool IsTransitionable();

    bool IsSightTarget();
}