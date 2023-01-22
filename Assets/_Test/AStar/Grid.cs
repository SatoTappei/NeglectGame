using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*で使うノードで構成されたグリッドのクラス
/// </summary>
public class Grid : MonoBehaviour
{
    // 変更すると直径が1ではなくなるので、色々な不具合が出るかもしれない
    readonly float NodeRadius = 0.5f;

    // TODO:プレイヤーの参照をここに置くのは良くないので別の場所に移す
    [SerializeField] Transform _player;

    [Header("移動不可能なレイヤー")]
    [SerializeField] LayerMask _obstacleLayer;
    [Header("グリッドのサイズ")]
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridDepth;

    Node[,] _grid;
    // TODO:マルチスレッドで処理する場合は並列で同じ変数を使うことになってしまう？ので対処する
    HashSet<Node> _neighbourNodeSet;

    void Awake()
    {
        // 周囲8マスを格納するので初期容量は8固定
        _neighbourNodeSet = new HashSet<Node>(8);
        GenerateGrid();
    }

    float NodeDiameter() => NodeRadius * 2;

    void GenerateGrid()
    {
        _grid = new Node[_gridDepth, _gridWidth];

        for (int z = 0; z < _gridDepth; z++)
            for (int x = 0; x < _gridWidth; x++)
            {
                Vector3 pos = transform.position;
                // グリッドの中心 - グリッドの半分の長さ + ノードn個分 + ノードの半径(ノードの中央を基準にしたい)
                pos.x += -_gridWidth / 2 + x * NodeDiameter() + NodeRadius;
                pos.z += -_gridDepth / 2 + z * NodeDiameter() + NodeRadius;
                bool isWalkable = !Physics.CheckSphere(pos, NodeRadius, _obstacleLayer);

                _grid[z, x] = new Node(pos, isWalkable, x, z);
            }
    }

    internal Node GetNode(Vector3 pos)
    {
        // 原点(0,0)を中心にノードを生成したので、座標は負の値にもなる。
        // グリッドの左もしくは下半分に移動するとClamp01で0になってしまう。
        // そのため、幅と奥行きの半分を足して座標を調整している。
        float percentX = Mathf.Clamp01((pos.x + _gridWidth / 2) / _gridWidth);
        float percentZ = Mathf.Clamp01((pos.z + _gridDepth / 2) / _gridDepth);

        // 全体の何％の場所かで座標を決めることにより様々な大きさのグリッドに対応させる
        int x = Mathf.RoundToInt((_gridWidth - 1) * percentX);
        int z = Mathf.RoundToInt((_gridDepth - 1) * percentZ);

        return _grid[z, x];
    }

    internal HashSet<Node> GetNeighbourNodeSet(int gridX, int gridZ)
    {
        _neighbourNodeSet.Clear();

        for (int z = -1; z <= 1; z++)
            for (int x = -1; x <= 1; x++)
            {
                if (z == 0 && x == 0) continue;

                int neighbourX = gridX + x;
                int neighbourZ = gridZ + z;

                if (0 <= neighbourX && neighbourX < _gridWidth &&
                    0 <= neighbourZ && neighbourZ < _gridDepth)
                {
                    _neighbourNodeSet.Add(_grid[neighbourZ, neighbourX]);
                }
            }

        return _neighbourNodeSet;
    }

    internal List<Node> path;
    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, _gizmosGridSize);

        if(_grid != null)
        {
            Node playerNode = GetNode(_player.position);

            foreach (Node node in _grid)
            {
                // 参照型なので配列に新たに追加せずとも同じ参照ならという分岐が出来る
                if (playerNode == node)
                {
                    Gizmos.color = Color.cyan;
                }
                else
                {
                    Gizmos.color = node.IsWalkable ? Color.white : Color.red;
                    if (path != null)
                        if (path.Contains(node))
                            Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(node.Pos, Vector3.one * NodeDiameter() * .9f);
            }
        }
    }
}
