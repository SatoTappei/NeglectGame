using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// �_���W�������\������ʘH1�}�X�P�ʂ̃f�[�^
/// </summary>
internal class DungeonPassMassData
{
    internal enum ComponentShape
    {
        Pass,
        Corner,
        TJunction,
        Cross,
        PassEnd,
    }

    Direction _dir;
    ComponentShape _shape;
    GameObject _obj;
    int _connect;

    internal DungeonPassMassData(Direction dir, ComponentShape shape, GameObject obj, int connect)
    {
        _dir = dir;
        _shape = shape;
        _obj = obj;
        _connect = connect;
    }

    internal Direction Dir { get => _dir; }
    internal ComponentShape Shape { get => _shape; }
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

    internal void Replace(Direction dir, ComponentShape shape, GameObject obj, int connect)
    {
        _dir = dir;
        _shape = shape;
        _obj = obj;
        _connect = connect;
    }
}
