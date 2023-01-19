using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;
using ComponentShape = DungeonPassMassData.ComponentShape;

/// <summary>
/// 文字列に対応したダンジョンの通路を建てるコンポーネント
/// </summary>
public class DungeonPassBuilder : MonoBehaviour
{
    enum Command
    {
        Forward = 'F',
        RotRight = '+',
        RotLeft = '-',
        Save = '[',
        Load = ']',
    }

    readonly int MaxPassDist = 8;
    readonly int DecreaseDist = 2;
    readonly int SaveStackCap = 4;
    // 3回書き換えを基準に設定
    readonly int PassMassDicCap = 450;
    readonly int EdgePassSetCap = 100;

    [Header("通路を構成するプレハブ")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    DungeonPassDirectionCalculator _directionCalculator;
    DungeonPassBinaryCalculator _binaryCalculator;
    Dictionary<Vector3Int, DungeonPassMassData> _passMassDic;
    /// <summary>生成後に見た目を修正するために条件を満たした通路を保持しておく</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new();
        _directionCalculator = new();
        _binaryCalculator = new();
        _passMassDic = new Dictionary<Vector3Int, DungeonPassMassData>(PassMassDicCap);
        _fixPassSet = new HashSet<Vector3Int>(EdgePassSetCap);
    }

    internal IReadOnlyDictionary<Vector3Int, DungeonPassMassData> GetMassDataAll() => _passMassDic;

    internal void BuildDungeonPass(string str)
    {
        // セーブ/ロードのコマンド用
        Stack<CursorParam> saveStack = new Stack<CursorParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int dirVec = Vector3Int.forward;
        int dist = MaxPassDist;
        
        foreach (char command in str)
        {
            switch ((Command)command)
            {
                // 直線の通路を生成して通路の先に基準点を移す
                case Command.Forward:
                    GeneratePassStraight(currentPos, dirVec, dist);
                    currentPos = currentPos + dirVec * dist * _helper.PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                // 基準点を右に90°回転させる
                case Command.RotRight:
                    dirVec = GetRotate90(dirVec, isPositive: true);
                    break;
                // 基準点を左に90°回転させる
                case Command.RotLeft:
                    dirVec = GetRotate90(dirVec, isPositive: false);
                    break;
                // 現在の基準点をスタックに積む
                case Command.Save:
                    saveStack.Push(new CursorParam(currentPos, dirVec, dist));
                    //Debug.Log($"Push:{currentPos},{dir},{dist}");
                    break;
                // スタックから基準点を取り出してその位置に基準点を変更する
                case Command.Load:
                    if (saveStack.Count == 0) break;
                    CursorParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dirVec = param.DirVec;
                    dist = param.Dist;
                    //Debug.Log($"Pop:{currentPos},{dir},{dist}");
                    break;
            }
        }

        FixPassVisual();
    }

    void GeneratePassStraight(Vector3Int startPos, Vector3Int dirVec, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Vector3Int pos = startPos + dirVec * i * _helper.PrefabScale;

            if (_passMassDic.ContainsKey(pos)) continue;

            float rotY = _directionCalculator.GetPassStraightRotY(dirVec);
            GameObject go = Instantiate(_passPrefab, pos, Quaternion.Euler(0, rotY, 0), _parent);

            Direction dir = _helper.ConvertToDir(dirVec);
            DungeonPassMassData massData = new (dir, ComponentShape.Pass, go, 2);
            
            _passMassDic.Add(pos, massData);

            // 奇数個目のマスと始点と終点を専用のコレクションに追加する
            // 条件を消すと見た目の修正時の精度が上がるが処理負荷も跳ね上がる
            if ((i / 2 == 1) || i == 0 || i == dist - 1)
                _fixPassSet.Add(pos);
        }
    }

    /// <summary>渡された方向ベクトルから90度回転させた方向ベクトルを返す</summary>
    /// <param name="isPositive">trueだと前右後左の時計回り、falseだと反時計回り</param>
    Vector3Int GetRotate90(Vector3Int dirVec, bool isPositive)
    {
        if      (dirVec == Vector3Int.forward) return isPositive ? Vector3Int.right : Vector3Int.left;
        else if (dirVec == Vector3Int.right)   return isPositive ? Vector3Int.back : Vector3Int.forward;
        else if (dirVec == Vector3Int.back)    return isPositive ? Vector3Int.left : Vector3Int.right;
        else if (dirVec == Vector3Int.left)    return isPositive ? Vector3Int.forward : Vector3Int.back;

        Debug.LogError("上下左右以外の角度です。: " + dirVec);
        return Vector3Int.zero;
    }

    void FixPassVisual()
    {
        foreach(Vector3Int pos in _fixPassSet)
        {
            // その座標が前後左右どの方向に接続されているかで向きを変更
            // いくつ接続されているかで対応する見た目に変更する
            (int dirs, int count) = _helper.GetNeighbourBinary(pos, _passMassDic.Keys);

            switch (count)
            {
                // 行き止まり
                case 1:
                    Replace(_binaryCalculator.GetPassEndRotY(dirs), _passEndPrefab, ComponentShape.PassEnd);
                    break;
                // 角
                case 2 when !_binaryCalculator.IsPassStraight(dirs):
                    Replace(_binaryCalculator.GetCornerRotY(dirs), _cornerPrefab, ComponentShape.Corner);
                    break;
                // 丁字路
                case 3:
                    Replace(_binaryCalculator.GetTJunctionRotY(dirs), _tJunctionPrefab, ComponentShape.TJunction);
                    break;
                // 十字路
                case 4:
                    Replace(0, _crossPrefab, ComponentShape.Cross);
                    break;
            }

            void Replace(float rotY, GameObject go, ComponentShape shape)
            {
                // オブジェクトを置き換えるので以前のものを削除する
                Destroy(_passMassDic[pos].Obj);

                _passMassDic[pos].Replace(dir:     _helper.ConvertToDir(rotY),
                                          shape:   shape,
                                          obj:     Instantiate(go, pos, Quaternion.Euler(0, rotY, 0), _parent),
                                          connect: count);
            }
        }
    }

    /// <summary>部屋の出入口の正面のマスを操作するので先に部屋を生成しておく必要がある</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataAll)
    {
        foreach (KeyValuePair<Vector3Int, Direction> pair in roomEntranceDataAll)
        {
            Vector3Int roomPos = pair.Key;
            Direction roomDir = pair.Value;

            // 出入口の座標と部屋が向いている方向から部屋の正面の座標を求める
            Vector3Int frontPos = roomPos - _helper.ConvertToPos(roomDir);
            DungeonPassMassData frontmassData = _passMassDic[frontPos];

            // オブジェクトを置き換えるので以前のものを削除する
            Destroy(frontmassData.Obj);

            // 正面のマスの接続数を+1して、部屋の出入り口と繋がった見た目に変更する
            switch (++frontmassData.Connect)
            {
                // 十字路
                case 4:
                    Replace(_crossPrefab, 0);
                    break;
                // 丁字路
                case 3:
                    float rot = _directionCalculator.GetTJunctionRotY(roomDir, frontmassData.Dir, frontmassData.Shape);
                    Replace(_tJunctionPrefab, rot);
                    break;
                // 直線もしくは角
                case 2:
                    if (roomDir == frontmassData.Dir)
                        Replace(_passPrefab, _directionCalculator.GetPassStraightRotY(roomDir));
                    else
                        Replace(_cornerPrefab, _directionCalculator.GetCornerRotY(roomDir, frontmassData.Dir));
                    break;
            }

            void Replace(GameObject go, float rotY)
            {
                frontmassData.Obj = Instantiate(go, frontPos, Quaternion.Euler(0, rotY, 0), _parent);
            }
        }
    }
}
