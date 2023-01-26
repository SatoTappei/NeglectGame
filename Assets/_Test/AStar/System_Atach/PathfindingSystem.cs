using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// A*��p���Čo�H�T�����s���R���|�[�l���g
/// </summary>
public class PathfindingSystem : MonoBehaviour, IPathGetable
{
    [SerializeField] PathfindingGrid _grid;

    /// <summary>�o�H�T���Ŏg���܂킷�̂Ń����o�ϐ��ɂ��Ă���</summary>
    Stack<Vector3> _pathStack;

    void Awake()
    {
        _pathStack = new Stack<Vector3>(_grid.GridSize);
    }

    public Stack<Vector3> GetPathStack(Vector3 startPos, Vector3 targetPos)
    {
        return Pathfinding(startPos, targetPos);
    }

    // TODO:�]�T������΃p�X������UniTask���g���Ĕ񓯊������ɂ���
    // TODO:�]�T������Όy�ʉ������ăR���N�V�����Ԃ̈ڂ��ւ��𖳂���
    Stack<Vector3> Pathfinding(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = _grid.GetNode(startPos);
        Node targetNode = _grid.GetNode(targetPos);

        if (!startNode.IsMovable || !targetNode.IsMovable)
            return null;

        // �O���b�h�̕��Ɖ��s���̘a��2�{���̏����e�ʂ��m��
        // �K�x�ɏ�Q����z�u���A�O���b�h�̒[����[�܂ŒT�������Č��肵��
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
            Debug.LogError("�ړ���̃m�[�h������܂���B");
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

    /// <summary>�ڕW�n�_���珇�ɐe���l�߂Ă����̂�Stack���g�p����</summary>
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

        // ���������𑪂���@(SqrMagnitude)�ł͏�肭�����Ȃ�����
        // �Z���ӂ͎΂߈ړ�������B�΂߈ړ��̏ꍇ�̋�����1.4�{
        // �΂߈ړ������c��͒����Ń^�[�Q�b�g�܂ł̋������v�Z����
        if (dx < dz)
            return 14 * dx + 10 * (dz - dx);
        else
            return 14 * dz + 10 * (dx - dz);
    }
}
