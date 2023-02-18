using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NodeBase
{
    public string Name { get; protected set; } = "NoName";

    protected List<NodeBase> _children = new List<NodeBase>();

    protected Func<BehaviorTree.NodeStatus> OnEnter;
    protected Func<BehaviorTree.NodeStatus> OnTick;

    public BehaviorTree.NodeStatus LastStatus { get; protected set; } = BehaviorTree.NodeStatus.Unknown;

    public NodeBase(string name = "",
        Func<BehaviorTree.NodeStatus> onEnter = null,
        Func<BehaviorTree.NodeStatus> onTick = null)
    {
        Name = name;
        OnEnter = onEnter;
        OnTick = onTick;
    }

    public NodeBase Add<T>(string name,
        Func<BehaviorTree.NodeStatus> onEnter = null,
        Func<BehaviorTree.NodeStatus> onTick = null) where T : NodeBase, new()
    {
        T newNode = new T();
        newNode.Name = name;
        newNode.OnEnter = onEnter;
        newNode.OnTick = onTick;

        _children.Add(newNode);

        return newNode;
    }

    public NodeBase Add<T>(T newNode) where T : NodeBase
    {
        _children.Add(newNode);
        return newNode;
    }

    public virtual void Reset()
    {
        LastStatus = BehaviorTree.NodeStatus.Unknown;

        foreach(var child in _children)
        {
            child.Reset();
        }
    }

    public void Tick(float deltaTime)
    {
        bool tickedAnyNodes = OnTick1(deltaTime);

        if (!tickedAnyNodes) Reset();
    }

    protected virtual void OnEnter1()
    {
        if (OnEnter != null)
        {
            LastStatus = OnEnter.Invoke();
        }
        else
        {
            LastStatus = _children.Count > 0 ? 
                BehaviorTree.NodeStatus.InProgress : BehaviorTree.NodeStatus.Succeeded;
        }
    }

    protected virtual bool OnTick1(float deltaTime)
    {
        bool tickAnyNodes = false;

        // このノードに入るのは初めてですか?
        if (LastStatus == BehaviorTree.NodeStatus.Unknown)
        {
            OnEnter();
            tickAnyNodes = true;
        }

        // Tick関数を持っているか？
        if(OnTick != null)
        {
            LastStatus = OnTick.Invoke();
            tickAnyNodes = true;

            // 失敗したか成功したかで抜ける
            if (LastStatus != BehaviorTree.NodeStatus.InProgress) return tickAnyNodes;
        }

        if(_children.Count == 0)
        {
            if (OnTick == null)
            {
                LastStatus = BehaviorTree.NodeStatus.Succeeded;
            }

            return tickAnyNodes;
        }

        // 全ての子のTick()を実行する
        foreach(NodeBase child in _children)
        {
            // 子が進行中の場合は、カチカチ音をたてた後、早期に終了します
            if (child.LastStatus == BehaviorTree.NodeStatus.InProgress)
            {
                tickAnyNodes |= child.OnTick1(deltaTime);
                return tickAnyNodes;
            }

            // ノードの結果がすでに記録されている場合は無視します
            if (child.LastStatus != BehaviorTree.NodeStatus.Unknown)
            {
                continue;
            }

            tickAnyNodes |= child.OnTick1(deltaTime);

            // デフォルトで子のステータスを継承する
            LastStatus = child.LastStatus;

            if (child.LastStatus == BehaviorTree.NodeStatus.InProgress)
            {
                return tickAnyNodes;
            }
            // 子が失敗した場合の評価の続行
            else if (child.LastStatus == BehaviorTree.NodeStatus.Failed &&
                !ContinueEvaluatingIfChildFailed())
            {
                return tickAnyNodes;
            }
            // 子が成功した場合の評価の続行
            else if (child.LastStatus == BehaviorTree.NodeStatus.Succeeded &&
                !ContinueEvaluatingIfChildSucceeded())
            {
                return tickAnyNodes;
            }
        }

        OnTickedAllChildren();

        return tickAnyNodes;
    }

    protected virtual bool ContinueEvaluatingIfChildFailed()
    {
        return true;
    }

    protected virtual bool ContinueEvaluatingIfChildSucceeded()
    {
        return true;
    }

    protected virtual void OnTickedAllChildren()
    {

    }
}
