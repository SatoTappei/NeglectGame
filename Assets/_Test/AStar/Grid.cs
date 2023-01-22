using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*�Ŏg���m�[�h�ō\�����ꂽ�O���b�h�̃N���X
/// </summary>
public class Grid : MonoBehaviour
{
    // �ύX����ƒ��a��1�ł͂Ȃ��Ȃ�̂ŁA�F�X�ȕs����o�邩������Ȃ�
    readonly float NodeRadius = 0.5f;

    // TODO:�v���C���[�̎Q�Ƃ������ɒu���̂͗ǂ��Ȃ��̂ŕʂ̏ꏊ�Ɉڂ�
    [SerializeField] Transform _player;

    [Header("�ړ��s�\�ȃ��C���[")]
    [SerializeField] LayerMask _obstacleLayer;
    [Header("�O���b�h�̃T�C�Y")]
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridDepth;

    Node[,] _grid;
    // TODO:�}���`�X���b�h�ŏ�������ꍇ�͕���œ����ϐ����g�����ƂɂȂ��Ă��܂��H�̂őΏ�����
    HashSet<Node> _neighbourNodeSet;

    void Awake()
    {
        // ����8�}�X���i�[����̂ŏ����e�ʂ�8�Œ�
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
                // �O���b�h�̒��S - �O���b�h�̔����̒��� + �m�[�hn�� + �m�[�h�̔��a(�m�[�h�̒�������ɂ�����)
                pos.x += -_gridWidth / 2 + x * NodeDiameter() + NodeRadius;
                pos.z += -_gridDepth / 2 + z * NodeDiameter() + NodeRadius;
                bool isWalkable = !Physics.CheckSphere(pos, NodeRadius, _obstacleLayer);

                _grid[z, x] = new Node(pos, isWalkable, x, z);
            }
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

    internal List<Node> path;
    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, _gizmosGridSize);

        if(_grid != null)
        {
            Node playerNode = GetNode(_player.position);

            foreach (Node node in _grid)
            {
                // �Q�ƌ^�Ȃ̂Ŕz��ɐV���ɒǉ������Ƃ������Q�ƂȂ�Ƃ������򂪏o����
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
