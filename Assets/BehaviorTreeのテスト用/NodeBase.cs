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

        // ���̃m�[�h�ɓ���̂͏��߂Ăł���?
        if (LastStatus == BehaviorTree.NodeStatus.Unknown)
        {
            OnEnter();
            tickAnyNodes = true;
        }

        // Tick�֐��������Ă��邩�H
        if(OnTick != null)
        {
            LastStatus = OnTick.Invoke();
            tickAnyNodes = true;

            // ���s�����������������Ŕ�����
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

        // �S�Ă̎q��Tick()�����s����
        foreach(NodeBase child in _children)
        {
            // �q���i�s���̏ꍇ�́A�J�`�J�`�������Ă���A�����ɏI�����܂�
            if (child.LastStatus == BehaviorTree.NodeStatus.InProgress)
            {
                tickAnyNodes |= child.OnTick1(deltaTime);
                return tickAnyNodes;
            }

            // �m�[�h�̌��ʂ����łɋL�^����Ă���ꍇ�͖������܂�
            if (child.LastStatus != BehaviorTree.NodeStatus.Unknown)
            {
                continue;
            }

            tickAnyNodes |= child.OnTick1(deltaTime);

            // �f�t�H���g�Ŏq�̃X�e�[�^�X���p������
            LastStatus = child.LastStatus;

            if (child.LastStatus == BehaviorTree.NodeStatus.InProgress)
            {
                return tickAnyNodes;
            }
            // �q�����s�����ꍇ�̕]���̑��s
            else if (child.LastStatus == BehaviorTree.NodeStatus.Failed &&
                !ContinueEvaluatingIfChildFailed())
            {
                return tickAnyNodes;
            }
            // �q�����������ꍇ�̕]���̑��s
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
