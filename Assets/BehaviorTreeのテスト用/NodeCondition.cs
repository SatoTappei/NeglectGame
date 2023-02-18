using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCondition : NodeBase
{
    protected System.Func<bool> Condition;
    protected bool WasPreviouslyAbleToRun = false;

    public NodeCondition(string name, System.Func<bool> condition) :
        base(name)
    {
        Condition = condition;
    }

    protected BehaviorTree.NodeStatus EvaluateCondition()
    {
        bool canRun = Condition != null ? Condition.Invoke() : false;

        if(canRun != WasPreviouslyAbleToRun)
        {
            WasPreviouslyAbleToRun = canRun;

            foreach(NodeBase child in _children)
            {
                child.Reset();
            }
        }

        return canRun ? BehaviorTree.NodeStatus.InProgress : BehaviorTree.NodeStatus.Failed;
    }
}
