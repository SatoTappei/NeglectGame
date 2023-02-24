using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T������������������C���^�[�t�F�[�X
/// </summary>
public interface IPathfinding
{
    Stack<Vector3> GetPathToTargetPos(Vector3 startPos, Vector3 endPos);
}