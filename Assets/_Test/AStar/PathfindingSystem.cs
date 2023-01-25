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
    
    public Stack<Vector3> GetPathStack(Vector3 startPos, Vector3 targetPos)
    {
        return Pathfinding(startPos, targetPos);
    }

    // TODO:余裕があればパス検索をUniTaskを使って非同期処理にする
    Stack<Vector3> Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.GetNode(startPos);
        Node targetNode = _grid.GetNode(targetPos);

        if (!startNode.IsMovable || !targetNode.IsMovable)
            return null;

        // TODO:キャパシティの調整、現在はマンハッタン距離分だけ確保している
        // TODO:余裕があれば軽量化をしてコレクション間の移し替えを無くす
        int defaultCapacity = (int)(Mathf.Abs(targetPos.x - startPos.x) + Mathf.Abs(targetPos.z - startPos.z));
        HashSet<Node> openSet = new HashSet<Node>(defaultCapacity);
        HashSet<Node> closedSet = new HashSet<Node>(defaultCapacity);

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

    Stack<Vector3> GetPathStack(Node start, Node target)
    {
        // TODO:頻繁に呼ばれるのならQueueをメンバ変数に昇格させる
        Stack<Vector3> stack = new Stack<Vector3>();
        
        Node currentNode = target;
        while(currentNode != start)
        {
            stack.Push(currentNode.Pos);
            currentNode = currentNode.ParentNode;
        }

        return stack;
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
