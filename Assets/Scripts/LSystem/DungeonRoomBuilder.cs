using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// ダンジョンの通路に応じた部屋を立てるコンポーネント
/// </summary>
public class DungeonRoomBuilder : MonoBehaviour
{
    // インスペクターで登録している部屋の数の合計
    readonly int RoomEntranceDicCap = 16;
    // 部屋の大きさが最大5*5なので
    readonly int RoomRangeSetCap = 25;

    [Header("生成する部屋のデータ")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    /// <summary>部屋の出入り口の正面の通路の見た目を修正するので保持しておく</summary>
    Dictionary<Vector3Int, Direction> _roomEntranceDic;
    /// <summary>何度も部屋の範囲を格納するのでメンバ変数として保持しておく</summary>
    HashSet<Vector3Int> _roomRangeSet;

    internal IReadOnlyDictionary<Vector3Int, Direction> GetRoomEntranceDataAll() => _roomEntranceDic;

    void Awake()
    {
        _helper = new();
        _roomEntranceDic = new Dictionary<Vector3Int, Direction>(RoomEntranceDicCap);
        _roomRangeSet = new HashSet<Vector3Int>(RoomRangeSetCap);
    }

    /// <summary>通路の周囲に部屋を建てるので、先に通路を建てている必要がある</summary>
    internal void BuildDungeonRoom(IReadOnlyDictionary<Vector3Int, DungeonPassMassData> massDataAll)
    {
        // 部屋を建てる候補として通路の側の座標を辞書型で受け取る
        IReadOnlyCollection<Vector3Int> massPosAll = massDataAll.Keys as IReadOnlyCollection<Vector3Int>;
        Dictionary<Vector3Int, Direction> estimatePosDic = GetAvailablePosDic(massPosAll);
        HashSet <Vector3Int> alreadyBuildPosSet = new HashSet<Vector3Int>(massPosAll);

        int roomIndex = 0;
        foreach (KeyValuePair<Vector3Int, Direction> pair in estimatePosDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            Vector3Int pos = pair.Key;
            Direction dir = pair.Value;
            DungeonRoomData data = _roomDataArr[roomIndex];
            HashSet<Vector3Int> roomRangeSet = GetRoomRangeSet(pos, dir, data.Width, data.Depth);

            if (!IsAvailableRange(roomRangeSet, alreadyBuildPosSet)) continue;

            if (data.IsAvailable())
            {
                Instantiate(data.GetPrefab(), pos, _helper.ConvertToInverseRot(dir), _parent);
                // 部屋同士が重ならないように生成した部屋の座標をコレクションに格納していく
                foreach (Vector3Int v in roomRangeSet)
                    alreadyBuildPosSet.Add(v);

                _roomEntranceDic.Add(pos, dir);
            }
            else
            {
                // 生成する部屋が無ければループを抜ける
                if (++roomIndex > _roomDataArr.Length - 1) break;
            }
        }
    }

    /// <summary>通路の側の座標を調べて、部屋を建てられる座標の辞書型として返す</summary>
    Dictionary<Vector3Int, Direction> GetAvailablePosDic(IReadOnlyCollection<Vector3Int> massPosAll)
    {
        // 初期容量は通路の両脇分を想定して通路の数*2を用意しておく
        Dictionary<Vector3Int, Direction> dic = new Dictionary<Vector3Int, Direction>(massPosAll.Count * 2);

        foreach (Vector3Int pos in massPosAll)
        {
            (int dirs, _) = _helper.GetNeighbourBinary(pos, massPosAll);

            // 各方向に通路が無ければその方向を部屋を生成可能な場所として辞書に追加する
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryForward)) Add(Direction.Forward);
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryBack))    Add(Direction.Back);
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryLeft))    Add(Direction.Left);
            if (!_helper.IsConnectFromBinary(dirs, DungeonHelper.BinaryRight))   Add(Direction.Right);

            void Add(Direction dir)
            {
                Vector3Int placePos = pos + _helper.ConvertToPos(dir);
                // 重複チェック
                if (dic.ContainsKey(placePos)) return;
                dic.Add(placePos, dir);
            }
        }

        return dic;
    }

    /// <summary>生成する部屋の範囲を取得する</summary>
    HashSet<Vector3Int> GetRoomRangeSet(Vector3Int pos, Direction dir, int width, int depth)
    {
        // 繰り返し呼び出すメソッドなので何回もnewせずに空にして使いまわす
        _roomRangeSet.Clear();

        for (int i = 0; i < depth; i++)
        {
            Vector3Int center = pos;
            switch (dir)
            {
                case Direction.Forward:
                    center.z += i * _helper.PrefabScale;
                    Add(center, Vector3Int.right);
                    break;
                case Direction.Back:
                    center.z -= i * _helper.PrefabScale;
                    Add(center, Vector3Int.right);
                    break;
                case Direction.Left:
                    center.x -= i * _helper.PrefabScale;
                    Add(center, Vector3Int.forward);
                    break;
                case Direction.Right:
                    center.x += i * _helper.PrefabScale;
                    Add(center, Vector3Int.forward);
                    break;
            }
        }

        return _roomRangeSet;

        // 指定された座標とその上下もしくは左右の座標を追加していく
        void Add(Vector3Int center, Vector3Int dir)
        {
            // 原点を追加
            _roomRangeSet.Add(center);
            // 左右の幅分を追加
            for (int i = 1; i <= width / 2; i++)
            {
                _roomRangeSet.Add(center + dir * i * _helper.PrefabScale);
                _roomRangeSet.Add(center - dir * i * _helper.PrefabScale);
            }
        }
    }

    bool IsAvailableRange(IReadOnlyCollection<Vector3Int> roomRangeSet, 
                          IReadOnlyCollection<Vector3Int> alreadyBuildPosSet)
    {
        foreach (Vector3Int pos in roomRangeSet)
        {
            if (alreadyBuildPosSet.Contains(pos)) 
                return false;
        }

        return true;
    }
}
