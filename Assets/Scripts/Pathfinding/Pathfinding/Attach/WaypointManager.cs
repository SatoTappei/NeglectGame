using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �o�H�T���Ɏg�p����Waypoint���Ǘ�����R���|�[�l���g
/// </summary>
public class WaypointManager : MonoBehaviour
{
    static readonly int DicCapacity = Enum.GetValues(typeof(WaypointType)).Length;

    [Header("Waypoint���q�Ƃ��ēo�^����Ă���I�u�W�F�N�g")]
    [SerializeField] Transform _wayPointParent;

    Dictionary<WaypointType, List<Vector3>> _waypointDic = new(DicCapacity);

    /* 
     * �����̍ŗD��^�X�N
     *  Waypoint�̐����A�e�ւ̓o�^�A�Q�Ƃ��鑤��Waypoint�̐e���擾�܂ł͏o�����B 
     *  �������ꂽWaypoint���擾���ăf�[�^�Ƃ��Ď��悤�ɂ���
     */

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegisterWaypoint();
        }
    }

    void RegisterWaypoint()
    {
        foreach(Transform child in _wayPointParent)
        {
            if(child.TryGetComponent(out DungeonWaypoint component))
            {
                WaypointType type = component.Type;
                Vector3 pos = child.position;

                if (!_waypointDic.ContainsKey(type))
                {
                    _waypointDic.Add(type, new List<Vector3>());
                }

                _waypointDic[type].Add(pos);
            }
            else
            {
                Debug.LogError("Waypoint�Ƃ��Ďg�p�ł��܂���: " + child.gameObject.name);
            }
        }
    }

    // TODO:�O��Ɠ����n�_���S�[���Ƃ��đI��ł��܂��ƈړ����Ȃ��Ȃ�s�������̂łǂ��ɂ�����
    //int _prev = -1;

    //Vector3 ITargetSelectable.GetExitPos()
    //{
    //    throw new System.NotImplementedException();
    //}

    //Vector3 ITargetSelectable.GetNextWaypointPos()
    //{
    //    int r;
    //    while (true)
    //    {
    //        r = Random.Range(0, _targetArr.Length);
    //        if (r != _prev)
    //        {
    //            _prev = r;
    //            break;
    //        }
    //    }

    //    return _targetArr[r].position;
    //}

    //Vector3 ITargetSelectable.GetExitPos() => _exit.position;
}

// Waypoint�̐����̓_���W�������������s��
// �ʘH�A�����̓����A�K�i������̂Ŕ��ʂ��鉽�����K�v�A��̒ǉ����l����
// ���̍��W�̃��X�g�́�����Waypoint�Ƃ��������͂������Ȃ��̂�
// �����Ă�����Ǘ�������Ȃ�H
// 

// ������͐������ꂽWaypoint��ǂݎ��
// DungeonWaypointVisualizer => Presetner�ɐ���
// WaypointTarget => Presetner����ǂݎ��
// ���ԓI���������݂��� Waypoint���� => Target�œǂݍ���
// ��������̓_���W��������=>�p�X�t�@�C���h�̃O���b�h�����̏��ōs���΂����Ȃ̂Ŗ��Ȃ�