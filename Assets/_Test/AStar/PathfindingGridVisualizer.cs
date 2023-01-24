using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T���̃O���b�h�����M�Y���ɕ\������R���|�[�l���g
/// </summary>
public class PathfindingGridVisualizer : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [Header("�O���b�h�ʒu��\������L�����N�^�[")]
    [SerializeField] Transform _character;

    void OnDrawGizmos()
    {
        if (_pathfindingGrid == null || _character == null || _pathfindingGrid.Grid == null)
        {
            Debug.LogWarning("�O���b�h����\�����邽�߂̎Q�Ƃ��s�����Ă��܂��B");
            return;
        }

        Node characterNode = _pathfindingGrid.GetNode(_character.position);
        foreach (Node node in _pathfindingGrid.Grid)
        {
            if (characterNode == node)
                Gizmos.color = Color.green;
            else if (node.IsMovable)
                Gizmos.color = Color.white;
            else
                Gizmos.color = Color.red;

            Gizmos.DrawCube(node.Pos, Vector3.one * _pathfindingGrid.NodeDiameter() * .9f);
        }
    }
}
