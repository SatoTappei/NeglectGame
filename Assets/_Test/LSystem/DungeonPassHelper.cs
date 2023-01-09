using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonBuildingHelper.Direction;

/// <summary>
/// ���������_���W�����̒ʘH��M��N���X
/// </summary>
public class DungeonPassHelper
{
    // TODO:���̃N���X���̂��{���ɕK�v���ǂ�����������
    //      DungeonPassBuilder�N���X�ɓ����ł��Ȃ���

    // ���̒萔���̂��_�u���Ă���̂ŋ��ʂŎg����֗��N���X�ɂ���K�v������
    // �������͈����ŃX�P�[�����Ƃ邩
    readonly int PrefabScale = 3;

    /// <summary>�ׂɃI�u�W�F�N�g�����݂���������܂Ƃ߂ĕԂ�</summary>
    public HashSet<Direction> GetNeighbour(Vector3Int pos, ICollection<Vector3Int> coll)
    {
        //Debug.Log("-----");
        //Debug.Log(pos + Vector3Int.forward);
        //Debug.Log(pos + Vector3Int.back);
        //Debug.Log(pos + Vector3Int.right);
        //Debug.Log(pos + Vector3Int.left);
        //foreach (var v in coll)
        //{
        //    Debug.Log(v);
        //}
        //Debug.Log("-----");

        HashSet<Direction> dirSet = new HashSet<Direction>(4);
        if (coll.Contains(pos + Vector3Int.forward * PrefabScale)) dirSet.Add(Direction.Forward);
        if (coll.Contains(pos + Vector3Int.back * PrefabScale))    dirSet.Add(Direction.Back);
        if (coll.Contains(pos + Vector3Int.right * PrefabScale))   dirSet.Add(Direction.Right);
        if (coll.Contains(pos + Vector3Int.left * PrefabScale))    dirSet.Add(Direction.Left);
        //if (coll.Contains(pos + Vector3Int.forward * 3))
        //{
        //    Debug.Log("����");
        //    dirSet.Add(Direction.Forward);
        //}
        //if (coll.Contains(pos + Vector3Int.back * 3))
        //{
        //    Debug.Log("����");
        //    dirSet.Add(Direction.Back);
        //}
        //if (coll.Contains(pos + Vector3Int.right * 3))
        //{
        //    Debug.Log("����");
        //    dirSet.Add(Direction.Right);
        //}
        //if (coll.Contains(pos + Vector3Int.left * 3))
        //{
        //    Debug.Log("����");
        //    dirSet.Add(Direction.Left);
        //}

        return dirSet;
    }
}
