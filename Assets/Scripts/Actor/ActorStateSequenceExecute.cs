using System.Threading;

/// <summary>
/// �e��Sequence�����s����X�e�[�g�̃N���X
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine)
    {

    }

    protected override void Enter()
    {
        // ���̃X�e�[�g�ɑJ�ڂ��Ă���ۂɂ͎��E�̋@�\�͐؂��Ă���̂�
        // Sequence���s���ɈႤSightableType�̃I�u�W�F�N�g���n����邱�Ƃ͂Ȃ�

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightObject();

        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            // Sequence�̎��s
            Debug.Log("�󔭌�Sequence�����s");
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            //  �G�𔭌�����
            //      n%�̊m���Ō��ʂ�����/�����̃X�e�[�g�ɑJ��
            // Sequence�̎��s

            Debug.Log("�G����Sequence�����s");
        }
        else
        {
            // �������E�ɖ����̂�Explore�ɖ߂�
            Debug.Log("�T���ɖ߂�");
        }
    }
}
