using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T���̃O���b�h�����M�Y���ɕ\������R���|�[�l���g
/// </summary>
public class PathfindingGridVisualizer : MonoBehaviour
{
    // TODO:����Grid�N���X�ɂ�����̂��������Ɉڂ�
    void OnDrawGizmos()
    {
        //Gizmos.DrawWireCube(transform.position, _gizmosGridSize);

        //if (_grid != null)
        //{
        //    Node playerNode = GetNode(_player.position);

        //    foreach (Node node in _grid)
        //    {
        //        // �Q�ƌ^�Ȃ̂Ŕz��ɐV���ɒǉ������Ƃ������Q�ƂȂ�Ƃ������򂪏o����
        //        if (playerNode == node)
        //        {
        //            Gizmos.color = Color.cyan;
        //        }
        //        else
        //        {
        //            Gizmos.color = node.IsMovable ? Color.white : Color.red;
        //            //if (path != null)
        //            //    if (path.Contains(node))
        //            //        Gizmos.color = Color.black;
        //        }
        //        Gizmos.DrawCube(node.Pos, Vector3.one * NodeDiameter() * .9f);
        //    }
        //}
    }
}
