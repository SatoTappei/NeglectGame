using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A*を用いて経路探索を行うコンポーネント
/// </summary>
public class PathfindingSystem : MonoBehaviour, IPathfinding
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [Header("デバッグ用:移動できない箇所が渡されたときに視覚化する")]
    [SerializeField] GameObject _debugVisualizer;

    Stack<Vector3> IPathfinding.GetPathToWaypoint(Vector3 startPos, Vector3 targetPos)
    {
        return Pathfinding(startPos, targetPos);
    }

    // TODO:余裕があればパス検索をUniTaskを使って非同期処理にする
    // TODO:余裕があれば軽量化をしてコレクション間の移し替えを無くす
    Stack<Vector3> Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _pathfindingGrid.GetNode(startPos);
        Node targetNode = _pathfindingGrid.GetNode(targetPos);

        if (!startNode.IsMovable)
        {
            Debug.LogWarning("だめなとっから移動します");
            Instantiate(_debugVisualizer, startPos, Quaternion.identity);
            UnityEditor.EditorApplication.isPaused = true;
        }

        if (/*!startNode.IsMovable || */!targetNode.IsMovable)
        {
            Debug.LogError("移動できない座標です: From " + startPos + " To " + targetPos);
            
            Instantiate(_debugVisualizer, targetPos, Quaternion.identity);

            return null;
        }

        // グリッドの幅と奥行きの和の2倍分の初期容量を確保
        // 適度に障害物を配置し、グリッドの端から端まで探索させて決定した
        HashSet<Node> openSet = new HashSet<Node>(_pathfindingGrid.GridSize * 2);
        HashSet<Node> closedSet = new HashSet<Node>(_pathfindingGrid.GridSize * 2);

        openSet.Add(startNode);

        return Recursive(openSet, closedSet, startNode, targetNode);
    }

    Stack<Vector3> Recursive(HashSet<Node> openSet, HashSet<Node> closedSet, in Node startNode, in Node targetNode)
    {
        Node current = openSet.OrderBy(n => n.TotalCost)
                              .ThenBy(n => n.EstimateCost)
                              .FirstOrDefault();

        if (current == null)
        {
            Debug.LogError("移動先のノードがありません。");
            return null;
        }
        else if (current == targetNode)
        {
            return GetPathStack(startNode, targetNode);
        }

        openSet.Remove(current);
        closedSet.Add(current);

        foreach (Node neighbour in _pathfindingGrid.GetNeighbourNodeSet(current.GridX, current.GridZ))
        {
            if (!neighbour.IsMovable || closedSet.Contains(neighbour)) continue;

            int moveToNeighbour = Distance(current.GridX, current.GridZ, neighbour.GridX, neighbour.GridZ);
            int neighbourActualCost = current.ActualCost + moveToNeighbour + neighbour.PenaltyCost;

            if (neighbourActualCost < neighbour.ActualCost || !openSet.Contains(neighbour))
            {
                neighbour.ActualCost = neighbourActualCost;
                int moveToTarget = Distance(neighbour.GridX, neighbour.GridZ, targetNode.GridX, targetNode.GridZ);
                neighbour.EstimateCost = moveToTarget;
                neighbour.ParentNode = current;

                openSet.Add(neighbour);
            }
        }

        return Recursive(openSet, closedSet, startNode, targetNode);
    }

    /// <summary>目標地点から順に親を詰めていくのでStackを使用する</summary>
    Stack<Vector3> GetPathStack(Node start, Node target)
    {
        // 複数のオブジェクトが非同期処理で参照するので使いまわせない
        Stack<Vector3> pathStack = new Stack<Vector3>();

        Node currentNode = target;
        while(currentNode != start)
        {
            pathStack.Push(currentNode.Pos);
            currentNode = currentNode.ParentNode;
        }

        return pathStack;
    }

    int Distance(int gridX1, int gridZ1, int gridX2, int gridZ2)
    {
        int dx = Mathf.Abs(gridX1 - gridX2);
        int dz = Mathf.Abs(gridZ1 - gridZ2);

        // 直線距離を測る方法(SqrMagnitude)では上手くいかなかった
        // 短い辺は斜め移動させる。斜め移動の場合の距離は1.4倍
        // 斜め移動した残りは直線でターゲットまでの距離を計算する
        if (dx < dz)
            return 14 * dx + 10 * (dz - dx);
        else
            return 14 * dz + 10 * (dx - dz);
    }
}
