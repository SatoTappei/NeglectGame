/// <summary>
/// �X�e�[�g�}�V���Ŏg�p����A�e�X�e�[�g�̍s�����J�ڏ�������������C���^�[�t�F�[�X
/// </summary>
internal interface IStateControl
{
    // Controller���ɃC���^�[�t�F�[�X����������Ă���̂�
    // ���̃��\�b�h�Q��Controller�̋@�\���g�킹�Ă�����Ƃ��������ɂȂ�

    void PlayAnim(string name);
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

    //public void PlayLookAroundAnim();
    //public void PlayAppearAnim();
    //public void PlayPanicAnim();
    //public void PlayJoyAnim();
    //public void PlayAttackAnim();

    //// �����̃X�e�[�g����J�ڂ���
    //public bool IsTransitionToPanicState();
    //public bool IsTransitionToDeadState();

    //// �_�b�V�����I�������Ă΂��A�Ȃ񂩃t���O�̃I���I�t�Ƃ�����
    //public void RunEndable();

    //public void PlayDeadAnim();

    //// �������ŃX�e�[�g��n���Ă��΂����̂ł͂Ȃ����H�̃e�X�g
    //internal StateID GetStateTest();
}