using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ړ�����L�����N�^�[��MVP�Ŏ�������ׂ�Presenter
/// </summary>
public class PathfindingPresenter : MonoBehaviour, IMovable
{
    [SerializeField] PathfindingMove _pathfindingMove;
    [Header("IPathGetable�̃I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;

    // �����System���ɂ����t���Ă���
    IPathGetable _pathGetable;

    void Start()
    {
        //_pathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
    }

    public void MoveStart(Vector3 targetPos)
    {
        // �������Ŏ擾�͈ꎞ�I�ȏ��u�A����
        _pathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
        MoveToTarget(targetPos);
    }

    void MoveToTarget(Vector3 targetPos)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _pathfindingMove.MoveFollowPath(pathStack);
    }

    // �ړ��ɕK�v�Ȃ���
    //  �ړ���̃m�[�h���l�܂���Stack <= ����
    //  ���ۂɈړ����s���R���|�[�l���g
    //  �ǂ��Ɉړ����邩���肷��R���|�[�l���g
    //  ���ۂ̈ړ��̓X�e�[�g�}�V�����ōs��
    
    // PathfindingMove�R���|�[�l���g�̖���
    //  �������Ă΂ꂽ��ړI�̈ʒu�Ɉړ����A�R�[���o�b�N�����s����A����
    //      �ړ���
    //      �_�b�V�������邩
    //      �I�����R�[���o�b�N
    //      ���:�L�����Z������
    //  �ړ����ɃL�����Z���ł���悤�ɂ���(���炩�̃C���^���N�V�����̂���)
}
