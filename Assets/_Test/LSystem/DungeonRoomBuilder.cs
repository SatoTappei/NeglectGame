using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// ダンジョンの通路に応じた部屋を立てるコンポーネント
/// </summary>
public class DungeonRoomBuilder : MonoBehaviour
{
    readonly int RoomDicCap = 16;
    readonly int PlaceDicCap = 64;

    [Header("部屋のプレハブ")]
    [SerializeField] GameObject _roomPrefab;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _roomDic;

    void Awake()
    {
        _helper = new DungeonHelper();
        _roomDic = new Dictionary<Vector3Int, GameObject>(RoomDicCap);
    }

    /// <summary>部屋の生成を行う</summary>
    internal void GenerateRoom(IReadOnlyCollection<Vector3Int> passAll)
    {
        // 部屋を生成可能な座標の辞書を作成する
        Dictionary<Vector3Int, Direction> placeDic = new Dictionary<Vector3Int, Direction>(PlaceDicCap);
        InsertToDic(placeDic, passAll);

        foreach (var pair in placeDic)
        {
            Quaternion rot = GetInverseRot(pair.Value);
            Instantiate(_roomPrefab, pair.Key, rot, _parent);
        }
    }

    /// <summary>部屋が生成可能な場所を辞書に挿入する</summary>
    void InsertToDic(Dictionary<Vector3Int, Direction> dic, IReadOnlyCollection<Vector3Int> passAll)
    {
        foreach (Vector3Int pos in passAll)
        {
            // 周囲の埋まっているマスの方角を取得する
            (int dirs, _) = _helper.GetNeighbourInt(pos, passAll);

            // 各方向に通路が無ければその方向を部屋を生成可能な場所として辞書に追加する
            if ((dirs & _helper.BForward) != _helper.BForward) AddDic(Direction.Forward);
            if ((dirs & _helper.BBack) != _helper.BBack) AddDic(Direction.Back);
            if ((dirs & _helper.BLeft) != _helper.BLeft) AddDic(Direction.Left);
            if ((dirs & _helper.BRight) != _helper.BRight) AddDic(Direction.Right);

            void AddDic(Direction dir)
            {
                Vector3Int placePos = pos + GetSidePos(dir);
                // 重複チェック
                if (dic.ContainsKey(placePos)) return;
                dic.Add(placePos, dir);
            }
        }
    }

    /// <summary>方向と逆の回転を取得する</summary>
    Quaternion GetInverseRot(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Back:
                return Quaternion.Euler(0, 180, 0);
            case Direction.Left:
                return Quaternion.Euler(0, -90, 0);
            case Direction.Right:
                return Quaternion.Euler(0, 90, 0);
            default:
                Debug.LogError("列挙型Directionで定義されていない値です。: " + dir);
                return Quaternion.identity;
        }
    }

    /// <summary>方向に応じた1マス脇の座標を返す</summary>
    Vector3Int GetSidePos(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward:
                return Vector3Int.forward * _helper.PrefabScale;
            case Direction.Back:
                return Vector3Int.back * _helper.PrefabScale;
            case Direction.Left:
                return Vector3Int.left * _helper.PrefabScale;
            case Direction.Right:
                return Vector3Int.right * _helper.PrefabScale;
            default:
                Debug.LogError("列挙型Directionで定義されていない値です。: " + dir);
                return Vector3Int.zero;
        }
    }
}
