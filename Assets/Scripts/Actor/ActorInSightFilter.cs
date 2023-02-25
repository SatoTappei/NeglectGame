using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎��E�ɉf�������̂��ړ����邱�Ƃ̂ł���Ώۂ����f����N���X
/// </summary>
public class ActorInSightFilter
{
    /// <summary>�_���W�����𐶐�����ۂɃC���X�y�N�^�[�Ŋ��蓖�Ă������̐��������e�ʂƂ��Đݒ�</summary>
    static readonly int UnAvailableMovingTargetListCap = 16;

    Dictionary<SightableType, List<Vector3>> _unAvailableMovingTargetDic;

    public ActorInSightFilter()
    {
        _unAvailableMovingTargetDic = new();
        foreach(SightableType type in Enum.GetValues(typeof(SightableType)))
        {
            _unAvailableMovingTargetDic.Add(type, new List<Vector3>(UnAvailableMovingTargetListCap));
        }
    }

    /// <returns>inSightObject �������� null</returns>
    public SightableObject FilteringAvailableMoving(SightableObject inSightObject)
    {
        List<Vector3> list = _unAvailableMovingTargetDic[inSightObject.SightableType];
        if (list.Contains(inSightObject.transform.position))
        {
            return null;
        }
        else
        {
            return inSightObject;
        }
    }

    public void AddUnAvailableMovingTarget(SightableObject inSightObject)
    {
        List<Vector3> list = _unAvailableMovingTargetDic[inSightObject.SightableType];
        list.Add(inSightObject.transform.position);
    }
}
