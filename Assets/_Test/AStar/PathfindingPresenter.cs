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

    // 移動に必要なもの
    //  移動先のノードが詰まったStack <= おｋ
    //  実際に移動を行うコンポーネント
    //  どこに移動するか決定するコンポーネント
    //  実際の移動はステートマシン内で行う
    
    // PathfindingMoveコンポーネントの役割
    //  処理が呼ばれたら目的の位置に移動し、コールバックを実行する、だけ
    //      移動先
    //      ダッシュさせるか
    //      終了時コールバック
    //      作る:キャンセル処理
    //  移動中にキャンセルできるようにする(何らかのインタラクションのため)
}
