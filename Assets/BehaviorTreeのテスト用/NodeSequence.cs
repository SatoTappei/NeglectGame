using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSequence : NodeBase
{
    protected override bool ContinueEvaluatingIfChildFailed()
    {
        return false;
    }

    protected override bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }

    protected override void OnTickedAllChildren()
    {
        LastStatus = _children[^1].LastStatus;
    }
}
