using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動するキャラクターをMVPで実装する為のPresenter
/// </summary>
public class PathfindingPresenter : MonoBehaviour, IMovable
{
    [SerializeField] PathfindingMove _pathfindingMove;
    [Header("IPathGetableのオブジェクトのタグ")]
    [SerializeField] string _tag;

    IPathGetable _pathGetable;

    void Start()
    {
        //_pathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
    }

    public void MoveStart(Vector3 targetPos)
    {
        // ↓ここで取得は一時的な処置、直す
        _pathGetable = GameObject.FindGameObjectWithTag(_tag).GetComponent<IPathGetable>();
        MoveToTarget(targetPos);
    }

    void MoveToTarget(Vector3 targetPos)
    {
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _pathfindingMove.MoveFollowPath(pathStack);
    }
}
