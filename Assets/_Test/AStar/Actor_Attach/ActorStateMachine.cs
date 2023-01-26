using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*��p�����L�����N�^�[�̃X�e�[�g�}�V��
/// </summary>
public class ActorStateMachine : MonoBehaviour
{
    [Header("System�ɂ��Ă���^�O")]
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
