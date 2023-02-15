using UnityEngine;

/// <summary>
/// ダンジョンを構成する通路1マス単位のデータ
/// </summary>
internal class DungeonPassMassData
{
    Direction _dir;
    ComponentShape _shape;
    GameObject _prefab;
    int _connect;

    internal DungeonPassMassData(Direction dir, ComponentShape shape, GameObject prefab, int connect)
    {
        _dir = dir;
        _shape = shape;
        _prefab = prefab;
        _connect = connect;
    }

    internal Direction Dir => _dir;
    internal ComponentShape Shape => _shape;
    internal GameObject Prefab { get => _prefab; set => _prefab = value; }
    internal int Connect { get => _connect; set => _connect = value; }

    internal void Replace(Direction dir, ComponentShape shape, GameObject obj, int connect)
    {
        _dir = dir;
        _shape = shape;
        _prefab = obj;
        _connect = connect;
    }
}
