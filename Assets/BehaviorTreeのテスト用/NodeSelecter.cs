using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSelecter : NodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }

    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return false;
    }

    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[^1].LastStatus;
    }
}
