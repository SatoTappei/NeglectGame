using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�L�����N�^�[���o�H�T���Ɏg�p����Waypoint���Ǘ�����N���X
/// </summary>
public class ActorPathfindingWaypoint
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> _waypointDic;
    List<Vector3> _unAvailabledRoomEntranceList;

    public ActorPathfindingWaypoint(IReadOnlyDictionary<WaypointType, List<Vector3>> waypointDic)
    {
        _waypointDic = waypointDic;
        _unAvailabledRoomEntranceList = new (waypointDic[WaypointType.Room].Count);
    }

    // TODO:�����E�F�C�|�C���g��A���Ŋl�����Ă��܂��̂������
    public Vector3 Get(WaypointType type)
    {
        List<Vector3> list = _waypointDic[type]; 
        Vector3 waypoint = list[Random.Range(0, list.Count)];

        return waypoint;
    }

    // TODO:�K�v�ɉ����ĕ����̏o������ȊO�ɑ΂��Ă����l�̏������o����悤�ɒ���
    //      ���̂Ƃ��땔���̏o������̂݃`�F�b�N���s����΂悢�̂ŏ��������b�v���������ɂȂ��Ă���
    public bool IsAvailable(Vector3 pos)
    {
        return !_unAvailabledRoomEntranceList.Contains(pos);
    }

    //void AddAvailableRoomEntrance(Vector3 pos)
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

    // ���_:�����ɓ���=>�����Ȃ�=>���낤��ɑJ��=>������������ �Ń��[�v���Ă��܂�
    
    // �m�F���������ɓ���Ȃ��悤�ɂ���ƁA�e�������m�F�����^�C�~���O�őS�ĉ����Ȃ������ꍇ
    //  �e�L�����N�^�[�̃^�X�N�������ł��Ȃ��Ȃ�

    // �e������S���m�F������܂��ŏ�����S���m�F���� �݂����ȃM�~�b�N���~����

    // �����̏o�����͎��o��Waypoint�����m���Ă���̂ł��̃N���X������ł��Ȃ��B

    // ���󕔉���Waypoint�͎��o�ł����F�m����Ă��Ȃ�
    // ����_���W�����̒������炵���N���Ă��Ȃ�
    // �o����Waypoint���烉���_���ɏo�Ă���悤�ɂ��A�_���W��������A��ۂ͂��̏o���Ɍ�����
}
