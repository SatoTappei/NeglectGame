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

    [Header("部屋のプレハブ")]
    [SerializeField] DungeonRoomData[] _roomDataArr;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _parent;

    [SerializeField] GameObject _test;

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

        //// ランダムに1箇所取得する
        //// 取得した箇所が生成不可能な箇所だった場合は再取得する必要がある
        //KeyValuePair<Vector3Int, Direction> paair = placeDic.ElementAtOrDefault(Random.Range(0, placeDic.Count));
        //DungeonRoomData data = _roomDataArr[0];
        //Quaternion rot = GetInverseRot(paair.Value);

        //Instantiate(data.GetPrefab(), paair.Key, rot, _parent);

        // 生成不可能な座標を保持しておくハッシュセット
        HashSet<Vector3Int> blockPosSet = new HashSet<Vector3Int>(BlockPosSetCap);

        // 
        int index = 0;
        // ランダムに並び替えた設置個所の辞書を順に走査する
        foreach (KeyValuePair<Vector3Int, Direction> pair in placeDic.OrderBy(_ => System.Guid.NewGuid()))
        {
            // 生成する部屋のデータ
            DungeonRoomData data = _roomDataArr[index];

            var ret = Check(pair.Key, pair.Value, data.Size, blockPosSet,passAll);

            // まずはその位置に必要な種類の部屋が生成できるか調べる
            if (ret.Item1)
            {
                Quaternion rot = GetInverseRot(pair.Value);
                Instantiate(data.GetPrefab(), pair.Key, rot, _parent);

                foreach(var v in ret.Item2)
                {
                    blockPosSet.Add(v);
                    //Instantiate(_test, v, Quaternion.identity);
                }
            }
            else
            {

            }
        }
    }

    (bool,HashSet<Vector3Int>) Check(Vector3Int pos, Direction dir, int size, IReadOnlyCollection<Vector3Int> blockPosSet, IReadOnlyCollection<Vector3Int> set2)
    {
        HashSet<Vector3Int> tempSet = new HashSet<Vector3Int>();

        // 左右と奥行を調べる
        switch (dir)
        {
            case Direction.Forward:
                // 奥行分繰り返す
                for(int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x, pos.y, pos.z + i * _helper.PrefabScale);
                    // 原点をブロックする箇所として追加
                    tempSet.Add(tempPos);
                    //Instantiate(_test, tempPos, Quaternion.identity);
                    // 左右の幅分をブロックする箇所として追加
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.left * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.right * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                        //Instantiate(_test, SideLeft, Quaternion.identity);
                        //Instantiate(_test, SideRight, Quaternion.identity);
                    }
                }
                break;
            case Direction.Back:
                // 奥行分繰り返す
                for (int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x, pos.y, pos.z - i * _helper.PrefabScale);
                    // 原点をブロックする箇所として追加
                    tempSet.Add(tempPos);

                    // 左右の幅分をブロックする箇所として追加
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.left * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.right * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                    }
                }
                break;
            case Direction.Left:
                // 奥行分繰り返す
                for (int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x - i * _helper.PrefabScale, pos.y, pos.z);
                    // 原点をブロックする箇所として追加
                    tempSet.Add(tempPos);

                    // 左右の幅分をブロックする箇所として追加
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.forward * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.back * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                    }
                }
                break;
            case Direction.Right:
                // 奥行分繰り返す
                for (int i = 0; i < size; i++)
                {
                    Vector3Int tempPos = new Vector3Int(pos.x + i * _helper.PrefabScale, pos.y, pos.z);
                    // 原点をブロックする箇所として追加
                    tempSet.Add(tempPos);

                    // 左右の幅分をブロックする箇所として追加
                    for (int j = 1; j <= size; j++)
                    {
                        Vector3Int SideLeft = tempPos + Vector3Int.forward * j * _helper.PrefabScale;
                        Vector3Int SideRight = tempPos + Vector3Int.back * j * _helper.PrefabScale;

                        tempSet.Add(SideLeft);
                        tempSet.Add(SideRight);
                    }
                }
                break;
        }

        bool flag = true;
        foreach(var v in tempSet)
        {
            bool b = !blockPosSet.Contains(v);
            bool bb = !set2.Contains(v);

            if (b && bb)
            {
                
            }
            else
            {
                flag = false;
            }
        }

        return (flag,tempSet);
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
