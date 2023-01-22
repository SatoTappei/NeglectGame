using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// A*を用いて経路探索を行うコンポーネント
/// </summary>
public class AStarPathfinding : MonoBehaviour
{
    [Header("Gridコンポーネントがアタッチされたオブジェクトのタグ")]
    [SerializeField] string _targetTag;
    [SerializeField] Transform seeker, target;

    Grid _grid;

    void Start()
    {
        // FindObjectOfTypeメソッドは処理負荷がオブジェクト数と比例するのでこっちを使う
        _grid = GameObject.FindGameObjectWithTag(_targetTag).GetComponent<Grid>();
    }

    void Update()
    {
        Pathfinding(seeker.position, target.position);
    }

    void Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.GetNode(startPos);
        Node targetNode = _grid.GetNode(targetPos);

        // TODO:キャパシティの調整、現在はマンハッタン距離分だけ確保している
        int defaultCapacity = (int)(Mathf.Abs(targetPos.x - startPos.x) + Mathf.Abs(targetPos.z - startPos.z));
        HashSet<Node> openSet = new HashSet<Node>(defaultCapacity);
        HashSet<Node> closedSet = new HashSet<Node>(defaultCapacity);

        openSet.Add(startNode);

        Recursive(openSet, closedSet, startNode, targetNode);
        //while (openSet.Count > 0)
        //{
            
        //}
    }

    void Recursive(HashSet<Node> openSet, HashSet<Node> closedSet, in Node startNode, in Node targetNode)
    {
        // TODO:総コストが一番低いノードを選ぶ、LINQでどうにか出来そう
        //Node currentNode = openSet[0];
        //for (int i = 1; i < openSet.Count; i++)
        //{
        //    if (openSet[i].TotalCost < currentNode.TotalCost ||
        //        openSet[i].TotalCost == currentNode.TotalCost &&
        //        openSet[i].EstimateCost < currentNode.EstimateCost)
        //    {
        //        currentNode = openSet[i];
        //    }
        //}
        Node currentNode = openSet.OrderBy(n => n.TotalCost)
                                  .ThenBy(n => n.EstimateCost)
                                  .FirstOrDefault();
        if (currentNode == null)
        {
            Debug.LogError("移動先のノードがありません。");
            return;
        }

        openSet.Remove(currentNode);
        closedSet.Add(currentNode);

        if (currentNode == targetNode)
        {
            RetracePath(startNode, targetNode);
            return;
        }

        foreach (Node neighbour in _grid.GetNeighbourNodeSet(currentNode.GridX, currentNode.GridZ))
        {
            if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
            {
                continue;
            }

            // TODO:変数名が長い
            // 現在のノードの実コスト + 隣のノードまでの距離
            //int newMovementCostToNeighbour = currentNode.ActualCost + Distance(currentNode, neighbour);
            int neighbourActualCost = currentNode.ActualCost + Distance(currentNode.Pos, neighbour.Pos);
            // 隣のマスの実コストより小さい、もしくはまだ開いていないノードなら
            if (neighbourActualCost < neighbour.ActualCost || !openSet.Contains(neighbour))
            {
                // 隣のマスの実コストにセット
                neighbour.ActualCost = neighbourActualCost;
                // 隣のマスの推定コストにこっからターゲットまでの距離を設定
                neighbour.EstimateCost = Distance(neighbour.Pos, targetNode.Pos);

                neighbour.ParentNode = currentNode;

                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
            }
        }

        Recursive(openSet, closedSet, startNode, targetNode);
    }

    void RetracePath(Node start, Node target)
    {
        List<Node> path = new List<Node>();
        Node currentNode = target;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        path.Reverse();

        _grid.path = path;
    }

    // TODO:2点間の距離をint型に直して返しているだけ、ちょっとこれでやってみる
    int Distance(Vector3 posA, Vector3 posB)
    {
        return (int)Vector3.SqrMagnitude(posA - posB);
        //int distX = Mathf.Abs(posA - gridX2);
        //int distZ = Mathf.Abs(gridZ1 - gridZ2);

        // 斜め移動を考慮した2点間の距離
        // 斜め移動は約1.4倍になるので長いほうの1.4倍をすれば斜め45°の長さになる
        // それに短い方の距離(斜め移動した後の残り)を足せばAからBへの距離になる
        // 返す際は10倍して小数部分を消している
        // TODO:他の距離の返し方、もしくはこのメソッド自体いらないかもしれないと疑う
        //if (distX > distZ)
        //{
        //    return 14 * distZ + 10 * (distX - distZ);
        //}
        //else
        //{
        //    return 14 * distX + 10 * (distZ - distX);
        //}
    }
}
