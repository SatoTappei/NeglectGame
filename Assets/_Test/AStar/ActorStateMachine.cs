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
