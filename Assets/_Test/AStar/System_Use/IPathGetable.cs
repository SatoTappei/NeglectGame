using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ړ����ɒH��m�[�h��Stack�Ŏ擾���鏈��������������C���^�[�t�F�[�X
/// </summary>
public interface IPathGetable
{
    public Stack<Vector3> GetPathStack(Vector3 _startPos, Vector3 _targetPos);
}
