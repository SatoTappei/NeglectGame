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
    readonly int BlockPosSetCap = 64;

    [Header("部屋のプレハブ")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
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

        //List<Vector3Int> blockPosSet = new List<Vector3Int>(BlockPosSetCap);
        int index = 0;
        foreach (KeyValuePair<Vector3Int, Direction> pair in placeDic)
        {
            // その座標に設置する部屋を決定する
            // 完全なランダムではなく、ボス部屋、お宝部屋など必要な全ての種類の部屋が1つ生成されるようにする
            // 必要な部屋が全部生成された後、マストじゃない部屋を生成する

            DungeonRoomData data = _roomDataArr[index];
            Quaternion rot = GetInverseRot(pair.Value);

            Instantiate(data.GetPrefab(), pair.Key, rot, _parent);

            // 他の部屋が占領している座標の場合は生成しない
            //if (blockPosSet.Contains(pair.Key)) continue;

            //Quaternion rot = GetInverseRot(pair.Value);

            //// ここから要リファクタリング
            //// 2*2の場合は計4マス、3*3の場合は計9マス調べる
            //for(int i = 0; i < _roomDataArr.Length; i++)
            //{
            //    DungeonRoomData room = _roomDataArr[i];

            //    if (room.MaxQuantity == -1)
            //    {
            //        GameObject go = Instantiate(room.GetPrefab(), pair.Key, rot, _parent);
            //        _roomDic.Add(pair.Key, go);
            //        break;
            //    }

            //    if (room.CheckAvailable())
            //    {
            //        if (room.Size > 1)
            //        {
            //            // ブロックする領域の計算
            //            int length = Mathf.CeilToInt(room.Size / 2);
            //            List<Vector3Int> tempList = new List<Vector3Int>();
            //            if (Fits(length, placeDic, pair, ref tempList))
            //            {
            //                blockPosSet.AddRange(tempList);
            //                var building = Instantiate(room.GetPrefab(), pair.Key, rot);
            //                _roomDic.Add(pair.Key, building);

            //                // 余白の部分も部屋として追加する
            //                foreach(var pos in tempList)
            //                {
            //                    _roomDic.Add(pos, building);
            //                }
            //            }

            //            GameObject go = Instantiate(room.GetPrefab(), pair.Key, rot, _parent);
            //            _roomDic.Add(pair.Key, go);
            //        }
            //        else
            //        {
            //            GameObject go = Instantiate(room.GetPrefab(), pair.Key, rot, _parent);
            //            _roomDic.Add(pair.Key, go);
            //        }
            //        break;
            //    }
            //}
            // リファクタリングここまで
        }
    }

    // リファクタリング必要メソッド
    bool Fits(int length, Dictionary<Vector3Int, Direction> placeDic,
        KeyValuePair<Vector3Int, Direction> pair, ref List<Vector3Int> tempList)
    {
        Vector3Int dir = Vector3Int.zero;
        // 前ももしくは後ろ向きの場合は左右にマージンが必要
        if (pair.Value == Direction.Forward || pair.Value == Direction.Back)
        {
            dir = Vector3Int.right;
        }
        else
        {
            // 左右向きの場合は上下にマージンが必要
            dir = new Vector3Int(0, 0, 1);
        }

        // その部屋の幅の半径分のループが必要
        for(int i = 1; i < length; i++)
        {
            // 中心から左右に調べていく
            Vector3Int pos1 = pair.Key + dir * i;
            Vector3Int pos2 = pair.Key - dir * i;

            // その位置が既に埋まっている場合はfalseを返す
            if (!placeDic.ContainsKey(pos1) || !placeDic.ContainsKey(pos2))
            {
                return false;
            }

            // 埋まっていない場合はそこに部屋を生成できるので一時保存リストに追加する
            tempList.Add(pos1);
            tempList.Add(pos2);
        }
        return true;
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
