using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        // FindObjectOfType���\�b�h�͏������ׂ��I�u�W�F�N�g���Ɣ�Ⴗ��̂ł��������g��
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

        // TODO:�L���p�V�e�B�̒����A���݂̓}���n�b�^�������������m�ۂ��Ă���
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
        // TODO:���R�X�g����ԒႢ�m�[�h��I�ԁALINQ�łǂ��ɂ��o������
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
            Debug.LogError("�ړ���̃m�[�h������܂���B");
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

            // TODO:�ϐ���������
            // ���݂̃m�[�h�̎��R�X�g + �ׂ̃m�[�h�܂ł̋���
            //int newMovementCostToNeighbour = currentNode.ActualCost + Distance(currentNode, neighbour);
            int neighbourActualCost = currentNode.ActualCost + Distance(currentNode.Pos, neighbour.Pos);
            // �ׂ̃}�X�̎��R�X�g��菬�����A�������͂܂��J���Ă��Ȃ��m�[�h�Ȃ�
            if (neighbourActualCost < neighbour.ActualCost || !openSet.Contains(neighbour))
            {
                // �ׂ̃}�X�̎��R�X�g�ɃZ�b�g
                neighbour.ActualCost = neighbourActualCost;
                // �ׂ̃}�X�̐���R�X�g�ɂ�������^�[�Q�b�g�܂ł̋�����ݒ�
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

    // TODO:2�_�Ԃ̋�����int�^�ɒ����ĕԂ��Ă��邾���A������Ƃ���ł���Ă݂�
    int Distance(Vector3 posA, Vector3 posB)
    {
        return (int)Vector3.SqrMagnitude(posA - posB);
        //int distX = Mathf.Abs(posA - gridX2);
        //int distZ = Mathf.Abs(gridZ1 - gridZ2);

        // �΂߈ړ����l������2�_�Ԃ̋���
        // �΂߈ړ��͖�1.4�{�ɂȂ�̂Œ����ق���1.4�{������Ύ΂�45���̒����ɂȂ�
        // ����ɒZ�����̋���(�΂߈ړ�������̎c��)�𑫂���A����B�ւ̋����ɂȂ�
        // �Ԃ��ۂ�10�{���ď��������������Ă���
        // TODO:���̋����̕Ԃ����A�������͂��̃��\�b�h���̂���Ȃ���������Ȃ��Ƌ^��
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
