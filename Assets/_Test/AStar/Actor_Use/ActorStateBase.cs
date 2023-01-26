using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃X�e�[�g�}�V���̊e�X�e�[�g�̊��N���X
/// </summary>
internal abstract class ActorStateBase
{
    protected IActorController _movable;

    protected enum Event
    {
        Enter,
        Stay,
        Exit,
    }

    public ActorStateBase(IActorController movable)
    {
        _movable = movable;
    }

    protected Event _event;

    protected virtual void Enter() => _event = Event.Stay;
    protected virtual void Stay() => _event = Event.Stay;
    protected virtual void Exit() => _event = Event.Exit;

    public void Update()
    {
        if      (_event == Event.Enter) Enter();
        else if (_event == Event.Stay)  Stay();
        else if (_event == Event.Exit)  Exit();
    }
}

/// <summary>
/// �^�[�Q�b�g�Ɍ������Ĉړ�����X�e�[�g�̃N���X
/// </summary>
internal class ActorStateMove : ActorStateBase
{
    public ActorStateMove(IActorController movable) : base(movable) { }

    protected override void Enter()
    {
        _movable.MoveToTarget();
        base.Enter();
    }
}
