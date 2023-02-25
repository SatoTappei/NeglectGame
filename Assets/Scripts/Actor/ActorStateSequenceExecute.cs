using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �e��Sequence�����s����X�e�[�g�̃N���X
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    static readonly float BattleWinPercent = 0.7f;

    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine) { }

    CancellationTokenSource _cts;

    protected override void Enter()
    {
        // ���̃X�e�[�g�ɑJ�ڂ��Ă���ۂɂ͎��E�̋@�\�͐؂��Ă���̂�
        // Sequence���s���ɈႤSightableType�̃I�u�W�F�N�g���n����邱�Ƃ͂Ȃ�
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();

        _cts = new CancellationTokenSource();

        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.Treasure);
            sequence.ExecuteAsync(_cts, () => 
            {
                TryChangeState(StateType.Goal);
            }).Forget();
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            if(Random.value < BattleWinPercent)
            {
                ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.BattleWin);
                sequence.ExecuteAsync(_cts, () =>
                {
                    TryChangeState(StateType.Goal);
                }).Forget();
            }
            else
            {
                ActorStateSequence sequence = _stateMachine.GetSequence(SequenceType.BattleLose);
                sequence.ExecuteAsync(_cts, () =>
                {
                    TryChangeState(StateType.Dead);
                }).Forget();
            }
        }
        else
        {
            // �������E�ɖ����̂�Explore�ɖ߂�
            Debug.LogWarning("����`�̗񋓌^�ł��B");
        }
    }

    protected override void Stay()
    {
        // �̗͂�0�ȉ��ɂȂ����玀��
    }

    protected override void Exit()
    {
        // ���̃X�e�[�g����J�ڂ��� = Sequence���ɂȂ񂩂���̃C�x���g����������Sequence���f
        _cts?.Cancel();
    }
}
