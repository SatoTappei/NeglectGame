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

    PathfindingDestination _pathfindingTargetDecider;
    IActorController _movable;

    void Start()
    {


        _movable = GetComponent<IActorController>();
        _pathfindingTargetDecider = GameObject.FindGameObjectWithTag(_tag).GetComponent<PathfindingDestination>();

        ActorStateMove actorStateMove = new ActorStateMove(_movable, _pathfindingTargetDecider);
        actorStateMove.Update();
    }

    void Update()
    {
        
    }
}
