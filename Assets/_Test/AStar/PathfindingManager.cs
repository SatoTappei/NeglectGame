using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����Ǘ�����R���|�[�l���g
/// </summary>
public class PathfindingManager : MonoBehaviour
{
    [SerializeField] PathfindingActorMove _actor;
    [SerializeField] PathfindingSystem _pathfindingSystem;

    void Start()
    {
        GetPath();
    }

    // ������ƃe�X�g
    void GetPath()
    {
        var v = _pathfindingSystem.Pathfinding(_actor.gameObject.transform.position, _actor.Target.position);
        _actor.Move(v);
    }
}
