using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// A*を用いて経路探索を行うコンポーネント
/// </summary>
public class PathfindingSystem : MonoBehaviour, IPathGetable
{
    [SerializeField] PathfindingGrid _grid;

    /// <summary>経路探索で使いまわすのでメンバ変数にしておく</summary>
    Stack<Vector3> _pathStack;

    void Awake()
    {
        _pathStack = new Stack<Vector3>(_grid.GridSize);
    }

    public Stack<Vector3> GetPathStack(Vector3 startPos, Vector3 targetPos)
    {
        return Pathfinding(startPos, targetPos);
    }

    // TODO:余裕があればパス検索をUniTaskを使って非同期処理にする
    // TODO:余裕があれば軽量化をしてコレクション間の移し替えを無くす
    Stack<Vector3> Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.GetNode(startPos);
        Node targetNode = _grid.GetNode(targetPos);

        if (!startNode.IsMovable || !targetNode.IsMovable)
            return null;

        // グリッドの幅と奥行きの和の2倍分の初期容量を確保
        // 適度に障害物を配置し、グリッドの端から端まで探索させて決定した
        HashSet<Node> openSet = new HashSet<Node>(_grid.GridSize * 2);
        HashSet<Node> closedSet = new HashSet<Node>(_grid.GridSize * 2);

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

        foreach (Node neighbour in _grid.GetNeighbourNodeSet(current.GridX, current.GridZ))
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
        _pathStack.Clear();
        
        Node currentNode = target;
        while(currentNode != start)
        {
            _pathStack.Push(currentNode.Pos);
            currentNode = currentNode.ParentNode;
        }

        return _pathStack;
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
