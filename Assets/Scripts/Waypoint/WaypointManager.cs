using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �o�H�T���Ɏg�p����Waypoint���Ǘ�����R���|�[�l���g
/// �e�L�����N�^�[�͂��̃N���X����Waypoint�̈ꗗ���擾���A�L�����N�^�[����Waypoint���Ǘ�����
/// </summary>
public class WaypointManager : MonoBehaviour, IWaypointManage
{
    static readonly int DicCapacity = Enum.GetValues(typeof(WaypointType)).Length;

    [Header("Waypoint���q�Ƃ��ēo�^����Ă���I�u�W�F�N�g")]
    [SerializeField] Transform _waypointParent;

    Dictionary<WaypointType, List<Vector3>> _waypointDic = new(DicCapacity);

    // TODO:Value��List��ǂݎ���p�ɃL���X�g����ƃL���X�g�s�\�G���[���o��̂��ǂ��ɂ�����
    //      List��ǂݎ���p�œn���Ă��Ȃ��̂�Dictionary��ǂݎ���p�ɂ��Ă��Ӗ����Ȃ�
    IReadOnlyDictionary<WaypointType, List<Vector3>> IWaypointManage.WaypointDic => _waypointDic;

    /// <summary>
    /// WayPoint�̐���ʒu���X�V���ꂽ�^�C�~���O�ŌĂԂ��ƂōX�V�\
    /// �L�����N�^�[��Dictionary���Q�Ƃ��Ă���̂ōX�V���Ă��L�����N�^�[���͂��̂܂܂ő��v
    /// </summary>
    public void RegisterWaypoint()
    {
        foreach(Transform child in _waypointParent)
        {
            if(child.TryGetComponent(out WaypointTag component))
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

    public List<Vector3> GetWaypointListWithWaypointType(WaypointType type) => _waypointDic[type];
}

