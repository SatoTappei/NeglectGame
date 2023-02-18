using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    public enum NodeStatus
    {
        Unknown,
        InProgress,
        Failed,
        Succeeded,
    }

    public NodeBase RootNode { get; private set; } = new NodeBase("ROOT");

    void Start()
    {
        RootNode.Reset();
    }

    void Update()
    {
        RootNode.Tick(Time.deltaTime);
    }
}
