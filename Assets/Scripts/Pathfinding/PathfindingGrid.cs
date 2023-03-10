using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索で使うノードで構成されたグリッドを管理するコンポーネント
/// </summary>
public class PathfindingGrid : MonoBehaviour
{
    /// <summary>
    /// 地形によって移動コストを設定するための構造体
    /// </summary>
    [System.Serializable]
    public struct TerrainData
    {
        [SerializeField] string _tag;
        [SerializeField] int _cost;

        public string Tag => _tag;
        public int Cost => _cost;
    }

    // 変更すると直径が1ではなくなるので、色々な不具合が出るかもしれない
    static readonly float NodeRadius = 0.5f;
    static readonly float NodeDiameter = NodeRadius * 2;

    [Header("デバッグ用:マスの位置を視覚化するためプレハブ")]
    [SerializeField] GameObject _testMassVisualizer;
    [Header("そのマスに侵入可能か判定するレイヤー")]
    [SerializeField] LayerMask _movableLayer;
    [SerializeField] LayerMask _unmovableLayer;
    [Header("グリッドのサイズ")]
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridDepth;
    [Header("地形とそのコストのデータ")]
    [SerializeField] TerrainData[] _terrainDatas;

    Dictionary<string, int> _terrainDataDic;
    // TODO:マルチスレッドで処理する場合は並列で同じ変数を使うことになってしまう？ので対処する
    HashSet<Node> _neighbourNodeSet;
    Node[,] _grid;

    internal int GridSize => _gridWidth + _gridDepth;

    void Awake()
    {
        _terrainDataDic = new Dictionary<string, int>(_terrainDatas.Length);
        // 周囲8マスを格納するので初期容量は8固定
        _neighbourNodeSet = new HashSet<Node>(8);

        foreach (TerrainData data in _terrainDatas)
        {
            _terrainDataDic.Add(data.Tag, data.Cost);
        }
    }

    /// <summary>このメソッドを外部から呼び出すことで経路探索用のグリッドを生成する</summary>
    public void GenerateGrid()
    {
        _grid = new Node[_gridDepth, _gridWidth];

        for (int z = 0; z < _gridDepth; z++)
            for (int x = 0; x < _gridWidth; x++)
            {
                Vector3 centerPos = GetNodeCenterPos(x, z);
                bool isMovable = IsMovableNode(centerPos);
                int penaltyCost = GetPenaltyCost(isMovable, centerPos);

                _grid[z, x] = new Node(centerPos, isMovable, x, z, penaltyCost);
            }
    }

    Vector3 GetNodeCenterPos(int x, int z)
    {
        Vector3 pos = transform.position;
        // グリッドの中心 - グリッドの半分の長さ + ノードn個分 + ノードの半径(ノードの中央を基準にしたい)
        pos.x += -_gridWidth / 2 + x * NodeDiameter + NodeRadius;
        pos.z += -_gridDepth / 2 + z * NodeDiameter + NodeRadius;

        return pos;
    }

    /// <summary>球状の当たり判定を出して障害物のレイヤーにヒットしなかったら移動できるのでtrueを返す</summary>
    bool IsMovableNode(Vector3 pos)
    {
        bool dontObstacle = !Physics.CheckSphere(pos, NodeRadius, _unmovableLayer);
        bool IsFloor = Physics.CheckSphere(pos, NodeRadius, _movableLayer);

        if (dontObstacle && IsFloor)
        {
            // デバッグ用、マスの位置を視覚化する
            if (_testMassVisualizer)
            {
                Instantiate(_testMassVisualizer, pos, Quaternion.identity);
            }
        }

        return dontObstacle && IsFloor;
    }

    int GetPenaltyCost(bool isMovable, Vector3 pos)
    {
        if (!isMovable) return -1;

        int penaltyCost = 0;

        // 下方向にRayを飛ばしてヒットしたオブジェクトのタグでコストの判定を行う
        Ray ray = new Ray(pos + Vector3.up * 50, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _movableLayer))
        {
            if (!_terrainDataDic.TryGetValue(hit.collider.gameObject.tag, out penaltyCost))
            {
                Debug.LogWarning("タグが存在しません: " + hit.collider.gameObject.tag);
            }    
        }

        return penaltyCost;
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
}
