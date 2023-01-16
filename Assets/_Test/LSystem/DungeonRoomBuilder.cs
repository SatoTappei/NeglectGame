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
    readonly int RoomDicCap = 16;
    readonly int PlaceDicCap = 64;
    readonly int BlockPosSetCap = 64;
    readonly int RoomRangeSetCap = 9;

    [Header("部屋のプレハブ")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _parent;

    [SerializeField] GameObject _test;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _roomDic;
    /// <summary>何度も部屋の範囲を格納するのでメンバ変数として保持しておく</summary>
    HashSet<Vector3Int> _roomRangeSet;
    
    void Awake()
    {
        _helper = new DungeonHelper();
        _roomDic = new Dictionary<Vector3Int, GameObject>(RoomDicCap);
        _roomRangeSet = new HashSet<Vector3Int>(RoomRangeSetCap);
    }

    /// <summary>部屋の生成を行う</summary>
    internal void GenerateRoom(IReadOnlyCollection<Vector3Int> passAll)
    {
        // 部屋を生成可能な座標の辞書を作成する
        Dictionary<Vector3Int, Direction> placeDic = new Dictionary<Vector3Int, Direction>(PlaceDicCap);
        InsertToDic(placeDic, passAll);

        // 生成不可能な座標を保持しておくハッシュセット
        HashSet<Vector3Int> blockPosSet = new HashSet<Vector3Int>(BlockPosSetCap);
        foreach (Vector3Int pos in passAll)
            blockPosSet.Add(pos);

        // ???何かに使う
        int index = 0;

        // ランダムに並び替えた設置個所の辞書を順に走査する
        foreach (KeyValuePair<Vector3Int, Direction> pair in placeDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            // 生成する部屋のデータ
            // 生成する部屋は必ず生成しなくてはいけない部屋が優先
            // 残ったところを穴埋めしていくように部屋を生成する
            DungeonRoomData data = _roomDataArr[index];

            HashSet<Vector3Int> roomRangeSet = GetRoomRangeSet(pair.Key, pair.Value, data.Width, data.Depth);

            if (IsAvailable(roomRangeSet, blockPosSet))
            {
                Quaternion rot = GetInverseRot(pair.Value);
                if (data.IsAvailable())
                {
                    Instantiate(data.GetPrefab(), pair.Key, rot, _parent);
                    // 部屋同士が重ならないように生成した部屋の座標をコレクションに格納していく
                    foreach (Vector3Int pos in roomRangeSet)
                        blockPosSet.Add(pos);
                }
                else
                {
                    index++;
                }
            }
        }
    }

    /// <summary>範囲内に既に部屋が無いかチェック</summary>
    bool IsAvailable(IReadOnlyCollection<Vector3Int> roomRangeSet, IReadOnlyCollection<Vector3Int> blockPosSet)
    {
        foreach (Vector3Int pos in roomRangeSet)
        {
            if (blockPosSet.Contains(pos)) 
                return false;
        }

        return true;
    }

    /// <summary>生成する部屋の範囲を取得する</summary>
    HashSet<Vector3Int> GetRoomRangeSet(Vector3Int pos, Direction dir, int width, int depth)
    {
        _roomRangeSet.Clear();

        for(int i = 0; i < depth; i++)
        {
            Vector3Int center = pos;
            switch (dir)
            {
                case Direction.Forward:
                    center.z += i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.left, Vector3Int.right);
                    break;
                case Direction.Back:
                    center.z -= i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.left, Vector3Int.right);
                    break;
                case Direction.Left:
                    center.x -= i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.forward, Vector3Int.back);
                    break;
                case Direction.Right:
                    center.x += i * _helper.PrefabScale;
                    AddSet(center, Vector3Int.forward, Vector3Int.back);
                    break;
            }
        }

        return _roomRangeSet;

        // 指定された座標とその上下もしくは左右の座標を追加していく
        void AddSet(Vector3Int center, Vector3Int dir1, Vector3Int dir2)
        {
            // 原点を追加
            _roomRangeSet.Add(center);
            // 左右の幅分を追加
            for (int i = 1; i <= width / 2; i++)
            {
                Vector3Int side1 = center + dir1 * i * _helper.PrefabScale;
                Vector3Int side2 = center + dir2 * i * _helper.PrefabScale;

                _roomRangeSet.Add(side1);
                _roomRangeSet.Add(side2);
            }
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
