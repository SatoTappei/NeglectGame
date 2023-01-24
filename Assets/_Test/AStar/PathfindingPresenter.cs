using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ړ�����L�����N�^�[��MVP�Ŏ�������ׂ�Presenter
/// </summary>
public class PathfindingPresenter : MonoBehaviour
{
    [SerializeField] PathfindingMove _pathfindingMove;
    [Header("�^�[�Q�b�g")]
    [SerializeField] Transform _target;
    [Header("IPathGetable�̃I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _tag;
    IPathGetable _IPathGetable;
    
    Vector3 _startPos;
    Vector3 _targetPos;

    void Start()
    {
        _startPos = transform.position;
        _IPathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
        Move();
    }

    void Move()
    {
        Stack<Vector3> pathStack = _IPathGetable.GetPathStack(_startPos, _target.position);
        _pathfindingMove.Move(pathStack);
    }
}
