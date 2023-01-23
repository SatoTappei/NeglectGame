using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 経路探索を管理するコンポーネント
/// </summary>
public class PathfindingManager : MonoBehaviour
{
    [SerializeField] PathfindingActorMove _actor;
    [SerializeField] PathfindingSystem _pathfindingSystem;

    void Start()
    {
        GetPath();
    }

    // ちょっとテスト
    void GetPath()
    {
        var v = _pathfindingSystem.Pathfinding(_actor.gameObject.transform.position, _actor.Target.position);
        _actor.Move(v);
    }
}
