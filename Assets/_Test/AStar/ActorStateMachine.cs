using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*を用いたキャラクターのステートマシン
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    [Header("Systemについているタグ")]
    [SerializeField] string _tag;

    PathfindingTargetDecider _pathfindingTargetDecider;
    IMovable _movable;

    void Start()
    {


        _movable = GetComponent<IMovable>();
        _pathfindingTargetDecider = GameObject.FindGameObjectWithTag(_tag).GetComponent<PathfindingTargetDecider>();

        ActorStateMove actorStateMove = new ActorStateMove(_movable, _pathfindingTargetDecider);
        actorStateMove.Update();
    }

    void Update()
    {
        
    }
}
