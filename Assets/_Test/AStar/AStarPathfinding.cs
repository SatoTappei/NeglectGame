using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// 時間検証用のSW、後で消す
using System.Diagnostics;
using System;

/// <summary>
/// A*を用いて経路探索を行うコンポーネント
/// </summary>
public class AStarPathfinding : MonoBehaviour
{
    // TODO:相互参照？になりそうなので依存関係を整理する
    [SerializeField] PathfindingManager _pathfindingManager;
    [Header("Gridコンポーネントがアタッチされたオブジェクトのタグ")]
    [SerializeField] string _targetTag;

    Grid _grid;
    // TODO: サンプル用、後で消す
    bool pathSuccess = false;
    
    void Start()
    {
        // FindObjectOfTypeメソッドは処理負荷がオブジェクト数に比例するのでこっちを使う
        _grid = GameObject.FindGameObjectWithTag(_targetTag).GetComponent<Grid>();
    }

    /// TODO:このメソッドいる？
    internal void StartPathfinding(Vector3 startPos, Vector3 endPos)
    {
        Pathfinding(startPos, endPos);
    }

    // TODO:本来ならパス検索を非同期処理にするべきだが、UniTaskを使ってリファクタリング必要
    void Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        // ↓パス検索のリファクタリング後に追加した処理
        Vector3[] wayPoints = new Vector3[0];
        pathSuccess = false;

        Node startNode = _grid.GetNode(startPos);
        Node targetNode = _grid.GetNode(targetPos);

        if (!startNode.IsMovable || !targetNode.IsMovable) return;

        // TODO:キャパシティの調整、現在はマンハッタン距離分だけ確保している
        // TODO:余裕があれば軽量化をしてコレクション間の移し替えを無くす
        int defaultCapacity = (int)(Mathf.Abs(targetPos.x - startPos.x) + Mathf.Abs(targetPos.z - startPos.z));
        HashSet<Node> openSet = new HashSet<Node>(defaultCapacity);
        HashSet<Node> closedSet = new HashSet<Node>(defaultCapacity);

        openSet.Add(startNode);

        // TODO:ここで再帰的に求めたパスが返ってくる
        //_grid.path = Recursive(openSet, closedSet, startNode, targetNode);
        wayPoints = Recursive(openSet, closedSet, startNode, targetNode);
        _pathfindingManager.FinishedProcessingPath(wayPoints, pathSuccess);
        sw.Stop();
        Debug.Log(sw.Elapsed);
    }

    //HashSet<Node> Recursive(HashSet<Node> openSet, HashSet<Node> closedSet, in Node startNode, in Node targetNode)
    Vector3[] Recursive(HashSet<Node> openSet, HashSet<Node> closedSet, in Node startNode, in Node targetNode)
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
            pathSuccess = true;
            return PathToHashSet(startNode, targetNode);
        }

        openSet.Remove(current);
        closedSet.Add(current);

        foreach (Node neighbour in _grid.GetNeighbourNodeSet(current.GridX, current.GridZ))
        {
            if (!neighbour.IsMovable || closedSet.Contains(neighbour)) continue;

            int moveToNeighbour = Distance(current.GridX, current.GridZ, neighbour.GridX, neighbour.GridZ);
            int neighbourActualCost = current.ActualCost + moveToNeighbour;

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

    // TODO: StackもしくはQueueに出来ないか検討する、そもそも先頭以外いらないのでは？
    //HashSet<Node> PathToHashSet(Node start, Node target)
    Vector3[] PathToHashSet(Node start, Node target)
    {
        //HashSet<Node> path = new HashSet<Node>();
        List<Node> path = new List<Node>();
        Node currentNode = target;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        //return path.Reverse().ToHashSet();
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    // TODO:このメソッドいらないのでは？
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dir = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 dir2 = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridZ - path[i].GridZ);
            // 方向が違うならば追加する、直線なら次のパスは飛ばす
            if(dir2 != dir)
            {
                waypoints.Add(path[i].Pos);
            }
            dir = dir2;
        }

        return waypoints.ToArray();
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
