using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �H��m�[�h��Stack���擾���鏈�����錾���ꂽ�C���^�[�t�F�[�X
/// </summary>
public interface IPathGetable
{
    public Stack<Vector3> GetPathStack(Vector3 _startPos, Vector3 _targetPos);
}
