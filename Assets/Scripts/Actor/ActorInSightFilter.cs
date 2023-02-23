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

    // TODO:�K�v�ɉ����ĕ����̏o������ȊO�ɑ΂��Ă����l�̏������o����悤�ɒ���
    //      ���̂Ƃ��땔���̏o������̂݃`�F�b�N���s����΂悢�̂ŏ��������b�v���������ɂȂ��Ă���
    //public bool IsAvailable(Vector3 pos)
    //{
    //    return !_unAvailabledRoomEntranceList.Contains(pos);
    //}

    //public void AddUnAvailableRoomEntrance(Vector3 pos)
    //{
    //    if (_unAvailabledRoomEntranceList.Contains(pos)) return;

    //    // �����̏o������ȊO�̍��W���n����Ă���\��������̂�
    //    // ���̍��W�������̏o������̃��X�g�Ɋ܂܂�Ă��邩�`�F�b�N����
    //    bool isRoomEntrance = _waypointDic[WaypointType.Room].Contains(pos);
    //    if (isRoomEntrance)
    //    {
    //        if (_unAvailabledRoomEntranceList.Count == _waypointDic[WaypointType.Room].Count - 1)
    //        {
    //            _unAvailabledRoomEntranceList.Clear();
    //        }

    //        _unAvailabledRoomEntranceList.Add(pos);
    //        return;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("�����̏o������ȊO�̍��W�𕔉��̓����Ƃ��Ē��ׂ悤�Ƃ��Ă��܂�: " + pos);
    //        return;
    //    }
    //}
}
