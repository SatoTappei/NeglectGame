using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの各行動を制御するコンポーネント
/// </summary>
public class ActorController : MonoBehaviour, IActorController
{
    [SerializeField] ActorMove _pathfindingMove;
    [Header("Systemオブジェクトのタグ")]
    [SerializeField] string _tag;

    PathfindingTarget _pathfindingTarget;
    IPathGetable _pathGetable;

    void Start()
    {
        // TODO:この参照先の取得方法がなんか気になる
        GameObject system = GameObject.FindGameObjectWithTag(_tag);
        _pathfindingTarget = system.GetComponent<PathfindingTarget>();
        _pathGetable = system.GetComponent<IPathGetable>();
    }

    public void MoveToTarget()
    {
        Vector3 targetPos = _pathfindingTarget.GetPathfindingTarget();
        Stack<Vector3> pathStack = _pathGetable.GetPathStack(transform.position, targetPos);
        _pathfindingMove.MoveFollowPath(pathStack);
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
