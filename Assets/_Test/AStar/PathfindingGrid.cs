using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///�o�H�T���Ŏg���m�[�h�ō\�����ꂽ�O���b�h�̃N���X
/// </summary>
public class PathfindingGrid : MonoBehaviour
{
    /// <summary>
    /// �n�`�ɂ���Ĉړ��R�X�g��ݒ肷�邽�߂̍\����
    /// </summary>
    [System.Serializable]
    public struct TerrainData
    {
        [SerializeField] string _tag;
        [SerializeField] int _cost;

        public string Tag => _tag;
        public int Cost => _cost;
    }

    // �ύX����ƒ��a��1�ł͂Ȃ��Ȃ�̂ŁA�F�X�ȕs����o�邩������Ȃ�
    readonly float NodeRadius = 0.5f;

    [Header("�ړ��\�ȃ��C���[")]
    [SerializeField] LayerMask _movableLayer;
    [Header("�ړ��s�\�ȃ��C���[")]
    [SerializeField] LayerMask _obstacleLayer;
    [Header("�O���b�h�̃T�C�Y")]
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridDepth;
    [Header("�n�`�Ƃ��̃R�X�g�̃f�[�^")]
    [SerializeField] TerrainData[] _terrainDataArr;

    Dictionary<string, int> _terrainDataDic;
    // TODO:�}���`�X���b�h�ŏ�������ꍇ�͕���œ����ϐ����g�����ƂɂȂ��Ă��܂��H�̂őΏ�����
    HashSet<Node> _neighbourNodeSet;
    Node[,] _grid;

    internal IReadOnlyCollection<Node>[,] Grid => _grid as IReadOnlyCollection<Node>[,];

    void Awake()
    {
        _terrainDataDic = new Dictionary<string, int>(_terrainDataArr.Length);
        // ����8�}�X���i�[����̂ŏ����e�ʂ�8�Œ�
        _neighbourNodeSet = new HashSet<Node>(8);

        foreach (TerrainData data in _terrainDataArr)
            _terrainDataDic.Add(data.Tag, data.Cost);

        GenerateGrid();
    }

    void GenerateGrid()
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
        // �O���b�h�̒��S - �O���b�h�̔����̒��� + �m�[�hn�� + �m�[�h�̔��a(�m�[�h�̒�������ɂ�����)
        pos.x += -_gridWidth / 2 + x * NodeDiameter() + NodeRadius;
        pos.z += -_gridDepth / 2 + z * NodeDiameter() + NodeRadius;

        return pos;
    }

    public float NodeDiameter() => NodeRadius * 2;

    bool IsMovableNode(Vector3 pos) => !Physics.CheckSphere(pos, NodeRadius, _obstacleLayer);

    int GetPenaltyCost(bool isMovable, Vector3 pos)
    {
        if (isMovable) return -1;

        int penaltyCost = 0;

        // ��������Ray���΂��ăq�b�g�����I�u�W�F�N�g�̃^�O�ŃR�X�g�̔�����s��
        Ray ray = new Ray(pos + Vector3.up * 50, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, _movableLayer))
            if (!_terrainDataDic.TryGetValue(hit.collider.gameObject.tag, out penaltyCost))
                Debug.LogWarning("�^�O�����݂��܂���: " + hit.collider.gameObject.tag);

        return penaltyCost;
    }

    internal Node GetNode(Vector3 pos)
    {
        // ���_(0,0)�𒆐S�Ƀm�[�h�𐶐������̂ŁA���W�͕��̒l�ɂ��Ȃ�B
        // �O���b�h�̍��������͉������Ɉړ������Clamp01��0�ɂȂ��Ă��܂��B
        // ���̂��߁A���Ɖ��s���̔����𑫂��č��W�𒲐����Ă���B
        float percentX = Mathf.Clamp01((pos.x + _gridWidth / 2) / _gridWidth);
        float percentZ = Mathf.Clamp01((pos.z + _gridDepth / 2) / _gridDepth);

        // �S�̂̉����̏ꏊ���ō��W�����߂邱�Ƃɂ��l�X�ȑ傫���̃O���b�h�ɑΉ�������
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
