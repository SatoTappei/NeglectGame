using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Direction = DungeonBuildingHelper.Direction;

/// <summary>
/// ���������_���W�����̒ʘH��M��N���X
/// </summary>
public class DungeonPassHelper
{
    // TODO:���̃N���X���̂��{���ɕK�v���ǂ�����������
    //      DungeonPassBuilder�N���X�ɓ����ł��Ȃ���

    // �萔���̂��_�u���Ă���̂ŋ��ʂŎg����֗��N���X�ɂ���K�v������
    // �������͈����ŃX�P�[�����Ƃ邩
    readonly int PrefabScale = 3;
    readonly int bForward = 0b1000;
    readonly int bBack    = 0b0100;
    readonly int bLeft    = 0b0010;
    readonly int bRight   = 0b0001;

    /// <summary>�ׂɃI�u�W�F�N�g�����݂���������܂Ƃ߂ĕԂ�</summary>
    public HashSet<Direction> GetNeighbour(Vector3Int pos, ICollection<Vector3Int> coll)
    {
        HashSet<Direction> dirSet = new HashSet<Direction>(4);
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale)) dirSet.Add(Direction.Forward);
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))    dirSet.Add(Direction.Back);
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))   dirSet.Add(Direction.Right);
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))    dirSet.Add(Direction.Left);

        return dirSet;
    }

    /// <summary>
    /// �ׂɃI�u�W�F�N�g����������Ɛڑ������o�C�i���`���ŕԂ�
    /// �w�肳�ꂽ�������܂܂�Ă��邩�ǂ����̔���͂�������g��
    /// </summary>
    /// <returns>0b�O�㍶�E, �ڑ���</returns>
    public (int dirs, int count) GetNeighbourInt(Vector3Int pos, IReadOnlyCollection<Vector3Int> coll)
    {
        int dirs = 0b0000;
        int count = 0;
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale))
        {
            dirs += bForward;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))
        {
            dirs += bBack;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))
        {
            dirs += bLeft;
            count++;
        }
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))
        {
            dirs += bRight;
            count++;
        }

        return (dirs, count);
    }
}
