/// <summary>
/// ���E�ɓ������I�u�W�F�N�g�ɂ���Ď��̃X�e�[�g��I������X�e�[�g�̃N���X
/// </summary>
public class ActorStateSelect : ActorStateBase
{
    public ActorStateSelect(ActorStateMachine stateMachine) : base(stateMachine) { }

    protected override void Enter()
    {
        SightableObject inSightObject = _stateMachine.StateControl.GetInSightAvailableMovingTarget();
        
        //if (inSightObject == null)
        //{
        //    ChangeState(StateType.Explore);
        //    Debug.LogWarning("InSightSelect�X�e�[�g����inSightObject��null�Ȃ̂�Explore�X�e�[�g�ɑJ�ڂ��܂�");
        //    return;
        //}

        // ���t���[���ŃX�e�[�g�̏������Ă΂��܂ł�InSightObject�̒l���ω����Ȃ��悤��
        // ���E�̋@�\��؂��Ă���
        //_stateMachine.StateControl.ToggleSight(isActive: false);

        if (inSightObject.SightableType == SightableType.RoomEntrance)
        {
            ChangeState(StateType.EnterTheRoom);
        }
        else if (inSightObject.SightableType == SightableType.Treasure ||
                 inSightObject.SightableType == SightableType.Enemy)
        {
            Debug.Log("��/�G�𔭌�" + _stateMachine.gameObject.GetHashCode());
            //_stateMachine.transform.localScale = UnityEngine.Vector3.one * 2;
            //ChangeState(StateType.SequenceExecute);
        }
        else
        {
            Debug.LogWarning("SightableType�̒l�����������ł�: " + inSightObject.SightableType);
        }
    }
}
