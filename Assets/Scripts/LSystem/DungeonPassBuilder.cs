using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>基本は6だが、長い通路が欲しい場合は8</summary>
    static readonly int MaxPassDist = 6;
    /// <summary>部屋の生成数に影響を与えるので、基本は2で固定</summary>
    static readonly int DecreaseDist = 2;

    // 生成した通路1マスずつを格納するコレクションの初期容量を
    // LSystemで3回書き換え＆MaxPassDistが6を基準に設定
    static readonly int PassMassDicCap = 450;
    static readonly int FixPassSetCap = 70;
    // セーブのコマンドは文字列の書き換えルール次第でいくらでも可能性があるので
    // 最低限ダンジョンらしい分岐数になる初期容量を設定
    static readonly int SaveCommandStackCap = 4;

    [Header("通路を構成するプレハブ")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _prefabParent;

    DungeonHelper _helper = new();
    Dictionary<Vector3Int, DungeonPassMassData> _passMassDic = new (PassMassDicCap);
    /// <summary>生成後に見た目を修正するために条件を満たした通路を保持しておく</summary>
    HashSet<Vector3Int> _fixPassSet = new (FixPassSetCap);
    /// <summary>
    /// 通路の接続部と行き止まりにWaypointを設定するので
    /// 初期容量は修正候補のマスのコレクションの半分の容量があれば基本的には十分
    /// </summary>
    List<Vector3Int> _waypointList = new (FixPassSetCap/2);

    internal IReadOnlyDictionary<Vector3Int, DungeonPassMassData> PassMassDic => _passMassDic;
    internal IReadOnlyCollection<Vector3Int> GetWaypointAll() => _waypointList;

    internal void BuildDungeonPass(string str)
    {
        // セーブ/ロードのコマンド用
        Stack<CursorParam> saveStack = new (SaveCommandStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int dirVec = Vector3Int.forward;
        int passDist = MaxPassDist;
        
        foreach (char command in str)
        {
            switch ((Command)command)
            {
                // 直線の通路を生成して通路の先に基準点を移す
                case Command.Forward:
                    GeneratePassStraight(currentPos, dirVec, passDist);
                    currentPos = currentPos + dirVec * passDist * _helper.PrefabScale;
                    passDist -= DecreaseDist;
                    passDist = Mathf.Max(1, passDist);
                    break;
                // 基準点を右に90°回転させる
                case Command.RotRight:
                    dirVec = _helper.GetRotate90(dirVec, isPositive: true);
                    break;
                // 基準点を左に90°回転させる
                case Command.RotLeft:
                    dirVec = _helper.GetRotate90(dirVec, isPositive: false);
                    break;
                // 現在の基準点をスタックに積む
                case Command.Save:
                    saveStack.Push(new (currentPos, dirVec, passDist));
                    break;
                // スタックから基準点を取り出してその位置に基準点を変更する
                case Command.Load:
                    if (saveStack.Count == 0) break;
                    CursorParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dirVec = param.DirVec;
                    passDist = param.Dist;
                    break;
            }
        }
    }

    void GeneratePassStraight(Vector3Int startPos, Vector3Int dirVec, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Vector3Int pos = startPos + dirVec * i * _helper.PrefabScale;

            // 同じ座標に通路が生成されないようにチェック
            if (_passMassDic.ContainsKey(pos)) continue;

            Direction dir = _helper.ConvertToDirection(dirVec);
            float rotY = _helper.GetPassStraightRotY(dir);
            GameObject go = Instantiate(_passPrefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
            int connect = _helper.GetConnectedFromShape(ComponentShape.Pass);
            
            DungeonPassMassData massData = new (dir, ComponentShape.Pass, go, connect);
            
            _passMassDic.Add(pos, massData);

            // 奇数個目のマスと始点と終点を専用のコレクションに追加する
            // 条件を消すと見た目の修正時の精度が上がるが処理負荷も跳ね上がる
            if ((i / 2 == 1) || i == 0 || i == dist - 1)
            {
                _fixPassSet.Add(pos);
            }
        }
    }

    /// <summary>通路を修正するので先に通路を生成している必要がある</summary>
    internal void FixPassVisual()
    {
        foreach (Vector3Int pos in _fixPassSet)
        {
            // その座標が前後左右どの方向に接続されているかで向きを変更
            // いくつ接続されているかで対応する見た目に変更する
            (int binary, int connect) = _helper.GetNeighbourBinary(pos, _passMassDic.Keys);

            switch (connect)
            {
                // 行き止まり
                case 1:
                    Replace(_passEndPrefab, _helper.GetPassEndRotY(binary), ComponentShape.PassEnd);
                    _waypointList.Add(pos);
                    break;
                // 角
                case 2 when !_helper.IsPassStraight(binary):
                    Replace(_cornerPrefab, _helper.GetCornerRotY(binary), ComponentShape.Corner);
                    break;
                // 丁字路
                case 3:
                    Replace(_tJunctionPrefab, _helper.GetTJunctionRotY(binary), ComponentShape.TJunction);
                    _waypointList.Add(pos);
                    break;
                // 十字路
                case 4:
                    Replace(_crossPrefab, 0, ComponentShape.Cross);
                    _waypointList.Add(pos);
                    break;
            }

            void Replace(GameObject prefab, float rotY, ComponentShape shape)
            {
                // オブジェクトを置き換えるので以前のものを削除する
                Destroy(_passMassDic[pos].Obj);

                Direction dir = _helper.ConvertToDirection(rotY);
                GameObject go = Instantiate(prefab, pos, Quaternion.Euler(0, rotY, 0), _prefabParent);
                _passMassDic[pos].Replace(dir, shape, go, connect);
            }
        }
    }

    /// <summary>部屋の出入口の正面のマスを操作するので先に部屋を生成しておく必要がある</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataDic)
    {
        foreach (KeyValuePair<Vector3Int, Direction> pair in roomEntranceDataDic)
        {
            Vector3Int roomPos = pair.Key;
            Direction roomDir = pair.Value;

            // 出入口の座標と部屋が向いている方向から部屋の正面の座標を求める
            Vector3Int frontPos = roomPos - _helper.ConvertToPos(roomDir);
            DungeonPassMassData frontmassData = _passMassDic[frontPos];

            // 正面のマスの接続数を+1して、部屋の出入り口と繋がった見た目に変更する
            switch (++frontmassData.Connect)
            {
                // 十字路
                case 4:
                    Replace(_crossPrefab, 0);
                    break;
                // 丁字路
                case 3:
                    float rot = _helper.GetTJunctionRotY(roomDir, frontmassData.Dir, frontmassData.Shape);
                    Replace(_tJunctionPrefab, rot);
                    break;
                // 直線もしくは角
                case 2:
                    if (roomDir == frontmassData.Dir)
                    {
                        Replace(_passPrefab, _helper.GetPassStraightRotY(roomDir));
                    }
                    else
                    {
                        Replace(_cornerPrefab, _helper.GetCornerRotY(roomDir, frontmassData.Dir));
                    }
                    break;
            }

            void Replace(GameObject prefab, float rotY)
            {
                // オブジェクトを置き換えるので以前のものを削除する
                Destroy(frontmassData.Obj);

                frontmassData.Obj = Instantiate(prefab, frontPos, Quaternion.Euler(0, rotY, 0), _prefabParent);
            }
        }
    }
}
