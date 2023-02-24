using Cysharp.Threading.Tasks;

/// <summary>
/// �e��Sequence�����s����X�e�[�g�̃N���X
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        // ���̃X�e�[�g�ɑJ�ڂ��Ă���ۂɂ͎��E�̋@�\�͐؂��Ă���̂�
        // Sequence���s���ɈႤSightableType�̃I�u�W�F�N�g���n����邱�Ƃ͂Ȃ�

        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();

        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.Treasure);
            sequence.ExecuteAsync(_stateMachine.gameObject.GetCancellationTokenOnDestroy(), () => 
            {
                TryChangeState(StateType.Goal);
            }).Forget();
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            //  �G�𔭌�����
            //      n%�̊m���Ō��ʂ�����/�����̃X�e�[�g�ɑJ��
            // Sequence�̎��s

            ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.BattleWin);
            sequence.ExecuteAsync(_stateMachine.gameObject.GetCancellationTokenOnDestroy(), () => 
            {
                TryChangeState(StateType.Dead);
            }).Forget();
        }
        else
        {
            // �������E�ɖ����̂�Explore�ɖ߂�
            Debug.Log("�T���ɖ߂�");
        }
    }
}
