using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �eWaypoint�������^�ŊǗ�����v���p�e�B������������C���^�[�t�F�[�X
/// �L�����N�^�[���ł��̃C���^�[�t�F�[�X����Ď擾����
/// </summary>
public interface IWaypointManage
{
    IReadOnlyDictionary<WaypointType, List<Vector3>> WaypointDic { get; }
}
