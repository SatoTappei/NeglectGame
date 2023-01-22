using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// ���Ԍ��ؗp��SW�A��ŏ���
using System.Diagnostics;

/// <summary>
/// A*��p���Čo�H�T�����s���R���|�[�l���g
/// </summary>
public class AStarPathfinding : MonoBehaviour
{
    [Header("Grid�R���|�[�l���g���A�^�b�`���ꂽ�I�u�W�F�N�g�̃^�O")]
    [SerializeField] string _targetTag;
    [SerializeField] Transform seeker, target;

    Grid _grid;

    void Start()
    {
        // FindObjectOfType���\�b�h�͏������ׂ��I�u�W�F�N�g���ɔ�Ⴗ��̂ł��������g��
        _grid = GameObject.FindGameObjectWithTag(_targetTag).GetComponent<Grid>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pathfinding(seeker.position, target.position);
        }
    }

    void Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = _grid.GetNode(startPos);
        Node targetNode = _grid.GetNode(targetPos);

        // TODO:�L���p�V�e�B�̒����A���݂̓}���n�b�^�������������m�ۂ��Ă���
        // TODO:�]�T������Όy�ʉ������ăR���N�V�����Ԃ̈ڂ��ւ��𖳂���
        int defaultCapacity = (int)(Mathf.Abs(targetPos.x - startPos.x) + Mathf.Abs(targetPos.z - startPos.z));
        HashSet<Node> openSet = new HashSet<Node>(defaultCapacity);
        HashSet<Node> closedSet = new HashSet<Node>(defaultCapacity);

        openSet.Add(startNode);

        // TODO:�����ōċA�I�ɋ��߂��p�X���Ԃ��Ă���
        _grid.path = Recursive(openSet, closedSet, startNode, targetNode);
        sw.Stop();
        Debug.Log(sw.Elapsed);
    }

    HashSet<Node> Recursive(HashSet<Node> openSet, HashSet<Node> closedSet, in Node startNode, in Node targetNode)
    {
        Node current = openSet.OrderBy(n => n.TotalCost)
                              .ThenBy(n => n.EstimateCost)
                              .FirstOrDefault();

        if (current == null)
        {
            Debug.LogError("�ړ���̃m�[�h������܂���B");
            return null;
        }
        else if (current == targetNode)
        {
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

    // TODO: Stack��������Queue�ɏo���Ȃ�����������A���������擪�ȊO����Ȃ��̂ł́H
    HashSet<Node> PathToHashSet(Node start, Node target)
    {
        HashSet<Node> path = new HashSet<Node>();
        Node currentNode = target;

        while(currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentNode;
        }

        return path.Reverse().ToHashSet();
    }

    int Distance(int gridX1, int gridZ1, int gridX2, int gridZ2)
    {
        int dx = Mathf.Abs(gridX1 - gridX2);
        int dz = Mathf.Abs(gridZ1 - gridZ2);

        // ���������𑪂���@(SqrMagnitude)�ł͏�肭�����Ȃ�����
        // �Z���ӂ͎΂߈ړ�������B�΂߈ړ��̏ꍇ�̋�����1.4�{
        // �΂߈ړ������c��͒����Ń^�[�Q�b�g�܂ł̋������v�Z����
        if (dx < dz)
            return 14 * dx + 10 * (dz - dx);
        else
            return 14 * dz + 10 * (dx - dz);
    }
}
