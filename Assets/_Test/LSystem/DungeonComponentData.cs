using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// �_���W�������\�����镔���ƒʘH1�}�X�P�ʂ̃f�[�^
/// </summary>
internal class DungeonComponentData
{
    // TODO:�����͌`��Ɛڑ����̃f�[�^������Ȃ��̂ŕʂ̃N���X�ɕ�����
    // TODO:�������������ł͎g���Ă��Ȃ��H�̂ŃN���X���Ƃ�����
    internal enum ComponentShape
    {
        Pass,
        Corner,
        TJunction,
        Cross,
        PassEnd,
        Room, // <= �ꎞ�I�ȃf�[�^�Ń��t�@�N�^�����O�����炢��Ȃ�
    }

    Vector3Int _pos;
    Direction _dir;
    ComponentShape _shape;
    GameObject _obj;
    int _connect;

    internal DungeonComponentData(Vector3Int pos, Direction dir, ComponentShape shape, GameObject obj, int connect)
    {
        _pos = pos;
        _dir = dir;
        _shape = shape;
        _obj = obj;
        _connect = connect;
    }

    internal Vector3Int Pos { get => _pos; set => _pos = value; }
    internal Direction Dir { get => _dir; set => _dir = value; }
    internal ComponentShape Shape { get => _shape; set => _shape = value; }
    internal GameObject Obj { get => _obj; set => _obj = value; }
    internal int Connect 
    { 
        get => _connect;
        set
        {
            if (value < 1 || 4 < value)
            {
                Debug.LogError("�ڑ�����1����4�ł�: " + value);
            }
            else
            {
                _connect = value;
            }
        } 
    }

    internal static Direction ConvertToDir(Vector3Int dirVec)
    {
        if      (dirVec == Vector3Int.forward) return Direction.Forward;
        else if (dirVec == Vector3Int.back)    return Direction.Back;
        else if (dirVec == Vector3Int.left)    return Direction.Left;
        else if (dirVec == Vector3Int.right)   return Direction.Right;
        else
        {
            Debug.LogError("�����x�N�g���̒l���s���ł�: " + dirVec);
            return Direction.Forward;
        }
    }

    internal static Direction ConvertToDir(float rotY)
    {
        if      (rotY == 0) return Direction.Forward;
        else if (rotY == 180) return Direction.Back;
        else if (rotY == -90) return Direction.Left;
        else if (rotY == 90) return Direction.Right;
        else
        {
            Debug.LogError("float�̒l���s���ł�: " + rotY);
            return Direction.Forward;
        }
    }
}
