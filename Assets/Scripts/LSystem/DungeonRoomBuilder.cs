using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョンの通路に応じた部屋を立てるコンポーネント
/// </summary>
public class DungeonRoomBuilder : MonoBehaviour
{
    /// <summary>インスペクターで割り当てた生成する部屋の数の合計を初期容量として確保する</summary>
    static readonly int RoomEntranceDicCap = 16;
    /// <summary>生成する部屋の最大の大きさである5*5を初期容量として確保する</summary>
    static readonly int RoomRangeListCap = 25;

    [Header("生成する部屋のデータ")]
    [SerializeField] DungeonRoomData[] _roomDatas;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _prefabParent;

    DungeonHelper _helper = new();
    /// <summary>部屋の出入り口の正面の通路の見た目を修正するので保持しておく</summary>
    Dictionary<Vector3Int, Direction> _roomEntranceDic = new (RoomEntranceDicCap);
    /// <summary>何度も部屋の範囲を格納するのでメンバ変数として保持しておく</summary>
    List<Vector3Int> _roomRangeList = new (RoomRangeListCap);

    internal IReadOnlyDictionary<Vector3Int, Direction> RoomEntranceDic => _roomEntranceDic;

    /// <summary>通路の周囲に部屋を建てるので、先に通路を建てている必要がある</summary>
    internal void BuildDungeonRoom(IReadOnlyDictionary<Vector3Int, DungeonPassMassData> passMassDic)
    {
        // 部屋を建てる候補として通路の側の座標を辞書型で受け取る
        IReadOnlyCollection<Vector3Int> passMassPositions = passMassDic.Keys as IReadOnlyCollection<Vector3Int>;
        IReadOnlyDictionary<Vector3Int, Direction> estimatePosDic = GetAvailablePosDic(passMassPositions);
        // 通路上に部屋を立てられないように通路の座標のコレクションをもとに生成する
        List<Vector3Int> alreadyBuildPosList = new (passMassPositions);

        // インスペクターで割り当てた順に、生成可能なランダムな位置に部屋を生成していく
        int roomIndex = 0;
        foreach (KeyValuePair<Vector3Int, Direction> pair in estimatePosDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            Vector3Int pos = pair.Key;
            Direction dir = pair.Value;

            DungeonRoomData roomData = _roomDatas[roomIndex];
            IReadOnlyCollection<Vector3Int> roomRanges = GetRoomRangeSet(pos, dir, roomData);
            
            if (IsAlreadyBuilded(roomRanges, alreadyBuildPosList)) continue;

            if (roomData.IsAvailable())
            {
                GameObject prefab = roomData.GetRandomVariationPrefab();
                Quaternion rot = _helper.ConvertToInverseRot(dir);
                Instantiate(prefab, pos, rot, _prefabParent);

                // 部屋同士が重ならないように生成した部屋の座標を追加する
                foreach (Vector3Int v in roomRanges)
                {
                    alreadyBuildPosList.Add(v);
                }

                _roomEntranceDic.Add(pos, dir);
            }
            else if (++roomIndex >= _roomDatas.Length)
            {
                break;
            }
        }
    }

    /// <summary>通路の側の座標を調べて、部屋を建てられる座標の辞書型として返す</summary>
    IReadOnlyDictionary<Vector3Int, Direction> GetAvailablePosDic(IReadOnlyCollection<Vector3Int> passMassPositions)
    {
        // 初期容量は通路の両脇分を想定して通路の数*2を用意しておく
        Dictionary<Vector3Int, Direction> estimatePosDic = new (passMassPositions.Count * 2);

        foreach (Vector3Int pos in passMassPositions)
        {
            (int binary, _) = _helper.GetNeighbourBinary(pos, passMassPositions);

            // 各方向に通路が無ければその方向を部屋を生成可能な場所として辞書に追加する
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryForward)) Add(Direction.Forward);
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryBack))    Add(Direction.Back);
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryLeft))    Add(Direction.Left);
            if (!_helper.IsConnectFromBinary(binary, DungeonHelper.BinaryRight))   Add(Direction.Right);

            void Add(Direction placeDir)
            {
                Vector3Int placePos = pos + _helper.ConvertToPos(placeDir);
                // 重複チェック
                if (estimatePosDic.ContainsKey(placePos)) return;
                estimatePosDic.Add(placePos, placeDir);
            }
        }

        return estimatePosDic;
    }

    /// <summary>生成する部屋の範囲を取得する</summary>
    IReadOnlyCollection<Vector3Int> GetRoomRangeSet(Vector3Int pos, Direction dir, DungeonRoomData roomData)
    {
        int depth = roomData.Depth;
        int width = roomData.Width;

        // 繰り返し呼び出すメソッドなので何回もnewせずに空にして使いまわす
        _roomRangeList.Clear();

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

        return _roomRangeList;

        // 指定された座標とその上下もしくは左右の座標を追加していく
        void Add(Vector3Int center, Vector3Int dir)
        {
            // 原点を追加
            _roomRangeList.Add(center);
            // 左右の幅分を追加
            for (int i = 1; i <= width / 2; i++)
            {
                _roomRangeList.Add(center + dir * i * _helper.PrefabScale);
                _roomRangeList.Add(center - dir * i * _helper.PrefabScale);
            }
        }
    }

    bool IsAlreadyBuilded(IReadOnlyCollection<Vector3Int> roomRangeSet, 
                          IReadOnlyCollection<Vector3Int> alreadyBuildPosSet)
    {
        foreach (Vector3Int pos in roomRangeSet)
        {
            if (alreadyBuildPosSet.Contains(pos)) 
                return true;
        }

        return false;
    }
}
