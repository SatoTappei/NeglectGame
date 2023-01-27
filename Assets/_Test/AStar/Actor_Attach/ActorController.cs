using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̊e�s���𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] Animator _anim;
    [SerializeField] ActorMove _actorMove;
    [Header("System�I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    //bool _isInit;

    void Start()
    {
        // TODO:���̎Q�Ɛ�̎擾���@���Ȃ񂩋C�ɂȂ�
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();
    }

    public void MoveToTarget(bool isDash)
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _actorMove.MoveFollowPath(pathStack, isDash);
    }

    public bool IsTransionMoveState()
    {
        // ���t���[���Ă΂��
        return false;
    }

    public void MoveCancel()
    {
        _actorMove.MoveCancel();
    }

    public bool IsTransionAnimationState()
    {
        return false;
        //if (_isInit)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    public void PlayAnim()
    {
        // TODO:�D��x(��) �A�j���[�V�������𕶎���Ŏw�肵�Ă���̂�Hash�ɒ���
        _anim.Play("Slash");
        // �A�j���[�V�������I�������A�C�h���ɑJ�ڂ�����
        // �ŏ��ɑS���̃X�e�[�g�𐶐����Ă����A�C�ӂ̃X�e�[�g�ɑJ�ڂł���悤�ɂ��Ă����K�v������
    }
}
