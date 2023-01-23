using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索のキャラ情報をギズモに表示するコンポーネント
/// </summary>
public class PathfindingActorVisualizer : MonoBehaviour
{
    //public void OnDrawGizmos()
    //{
    //    if (_path != null)
    //    {
    //        for (int i = _targetIndex; i < _path.Length; i++)
    //        {
    //            Gizmos.color = Color.black;
    //            Gizmos.DrawCube(_path[i], Vector3.one);

    //            if (i == _targetIndex)
    //            {
    //                Gizmos.DrawLine(transform.position, _path[i]);
    //            }
    //            else
    //            {
    //                Gizmos.DrawLine(_path[i - 1], _path[i]);
    //            }
    //        }
    //    }
    //}
}
