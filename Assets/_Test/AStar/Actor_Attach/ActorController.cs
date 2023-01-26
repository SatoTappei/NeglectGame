using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移動するキャラクターをMVPで実装する為のPresenter
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] ActorMove _pathfindingMove;
    [Header("IPathGetableのオブジェクトのタグ")]
    [SerializeField] string _tag;

    PathfindingDestination _pathfindingDestination;

    // これはSystem側にくっ付いている
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

    public void MoveStart()
    {
        throw new System.NotImplementedException();
    }

    public void MoveCancel()
    {
        throw new System.NotImplementedException();
    }

    public void PlayAnim()
    {
        throw new System.NotImplementedException();
    }
}
