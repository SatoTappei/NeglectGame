using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �e��Sequence�����s����X�e�[�g�̃N���X
/// </summary>
public class ActorStateSequenceExecute : ActorStateBase
{
    static readonly float BattleWinPercent = 0.1f;

    public ActorStateSequenceExecute(ActorStateMachine stateMachine) : base(stateMachine) { }

    CancellationTokenSource _cts;

    protected override void Enter()
    {
        _cts = new CancellationTokenSource();

        if (_stateMachine.StateControl.IsBelowHpThreshold())
        {
            ExecuteSequence(SequenceType.Exit, StateType.Goal);
            return;
        }

        // ���̃X�e�[�g�ɑJ�ڂ��Ă���ۂɂ͎��E�̋@�\�͐؂��Ă���̂�
        // Sequence���s���ɈႤSightableType�̃I�u�W�F�N�g���n����邱�Ƃ͂Ȃ�
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        if (inSightObject?.SightableType == SightableType.Treasure)
        {
            ExecuteSequence(SequenceType.Treasure, StateType.Goal);
        }
        else if (inSightObject?.SightableType == SightableType.Enemy)
        {
            if(Random.value < BattleWinPercent)
            {
                ExecuteSequence(SequenceType.BattleWin, StateType.Dead);
            }
            else
            {
                ExecuteSequence(SequenceType.BattleLose, StateType.Dead);
            }
        }
        else
        {
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

    void ExecuteSequence(SequenceType sequenceType, StateType transitionStateType)
    {
        ActorStateSequence sequence = _stateMachine.GetSequence(sequenceType);
        sequence.ExecuteAsync(_cts, () => TryChangeState(transitionStateType)).Forget();
    }
}
