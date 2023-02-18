using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public class BehaviorTreeSetup : MonoBehaviour
{
    [Header("Ç§ÇÎÇ§ÇÎê›íË")]
    [SerializeField] float _wanderRange = 10.0f;

    protected BehaviorTree LinkedBehaviorTree;
    protected CharacterAgent Agent;
    protected AwarenessSystem Sensors;

    void Awake()
    {
        LinkedBehaviorTree = GetComponent<BehaviorTree>();
        Agent = GetComponent<CharacterAgent>();
        Sensors = GetComponent<AwarenessSystem>();

        NodeBase behaviorTreeRoot = LinkedBehaviorTree.RootNode;

        var wanderRoot = behaviorTreeRoot.Add<NodeSequence>("Wander");
        wanderRoot.Add<NodeAction>("PerformWander", () =>
        {
            Vector3 location = Agent.PickLocationInRange(_wanderRange);
            Agent.MoveTo(location);
            return BehaviorTree.NodeStatus.InProgress;
        },
        () =>
        {
            return Agent.AtDestination ? BehaviorTree.NodeStatus.Succeeded : BehaviorTree.NodeStatus.InProgress;
        });
    }

    void Update()
    {
        
    }
}
