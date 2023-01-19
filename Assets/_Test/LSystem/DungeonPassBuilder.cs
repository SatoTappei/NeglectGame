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
    readonly int PassDicCap = 64;
    readonly int EdgePassSetCap = 16;

    [Header("通路を構成するプレハブ")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("生成したプレハブの親")]
    [SerializeField] Transform _parent;

    DungeonHelper _helper;
    DungeonPassHelper _pHelper;
    Dictionary<Vector3Int, DungeonPassMassData> _passMassDic;
    /// <summary>生成後に見た目を修正するために条件を満たした通路を保持しておく</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new();
        _pHelper = new();
        _passMassDic = new Dictionary<Vector3Int, DungeonPassMassData>(PassDicCap);
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
                    dirVec = RotDirVec90(dirVec, isPositive: true);
                    break;
                // 基準点を左に90°回転させる
                case Command.RotLeft:
                    dirVec = RotDirVec90(dirVec, isPositive: false);
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

            float rotY = _pHelper.GetPassStraightRotY(dirVec);
            GameObject go = Instantiate(_passPrefab, pos, Quaternion.Euler(0, rotY, 0), _parent);

            Direction dir = DungeonPassMassData.ConvertToDir(dirVec);
            DungeonPassMassData massData = new (pos, dir, ComponentShape.Pass, go, 2);
            
            _passMassDic.Add(pos, massData);

            // 奇数個目のマスと始点と終点を専用のコレクションに追加する
            // 条件を消すと見た目の修正時の精度が上がるが処理負荷も跳ね上がる
            if ((i / 2 == 1) || i == 0 || i == dist - 1)
                _fixPassSet.Add(pos);
        }
    }

    void FixPassVisual()
    {
        foreach(Vector3Int pos in _fixPassSet)
        {
            // その座標が前後左右どの方向に接続されているかで向きを変更
            // いくつ接続されているかで対応する見た目に変更する
            (int dirs, int count) = _helper.GetNeighbourBinary(pos, _passMassDic.Keys);
            bool dirForward = (dirs & _helper.BForward) == _helper.BForward;
            bool dirBack =    (dirs & _helper.BBack)    == _helper.BBack;
            bool dirLeft =    (dirs & _helper.BLeft)    == _helper.BLeft;
            bool dirRight =   (dirs & _helper.BRight)   == _helper.BRight;

            float rotY = 0;
            GameObject go = null;
            ComponentShape shape = ComponentShape.Pass;
            switch (count)
            {
                // 行き止まり
                case 1:
                    if      (dirForward) rotY = 180;
                    else if (dirRight)   rotY = -90;
                    else if (dirLeft)    rotY = 90;

                    go = _passEndPrefab;
                    shape = ComponentShape.PassEnd;
                    break;
                // 角
                case 2:
                    // 上下もしくは左右に接続されている場合は通路なので何もしない
                    if ((dirForward && dirBack) || (dirRight && dirLeft)) 
                        continue;

                    if      (dirForward && dirRight) rotY = 180;
                    else if (dirLeft && dirForward)  rotY = 90;
                    else if (dirRight && dirBack)    rotY = -90;

                    go = _cornerPrefab;
                    shape = ComponentShape.Corner;
                    break;
                // 丁字路
                case 3:
                    if      (dirForward && dirBack && dirLeft)  rotY = 90;
                    else if (dirForward && dirRight && dirLeft) rotY = 180;
                    else if (dirForward && dirBack && dirRight) rotY = -90;

                    go = _tJunctionPrefab;
                    shape = ComponentShape.TJunction;
                    break;
                // 十字路
                case 4:
                    go = _crossPrefab;
                    shape = ComponentShape.Cross;
                    break;
            }

            // 置き換えるので元あったオブジェクトは削除する
            Destroy(_passMassDic[pos].Obj);
            _passMassDic[pos].Dir = DungeonPassMassData.ConvertToDir(rotY);
            _passMassDic[pos].Shape = shape;
            _passMassDic[pos].Obj = Instantiate(go, pos, Quaternion.Euler(0, rotY, 0), _parent);
            _passMassDic[pos].Connect = count;
        }
    }

    /// <summary>部屋の出入口の正面のマスを操作するので先に部屋を生成しておく必要がある</summary>
    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> roomEntranceDataAll)
    {
        DungeonPassHelper dungeonCalc = new();

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
                    frontmassData.Obj = Instantiate(_crossPrefab, frontPos, Quaternion.identity, _parent);
                    break;
                // 丁字路
                case 3:
                    float rot3 = dungeonCalc.GetTJunctionRotY(roomDir, frontmassData.Dir, frontmassData.Shape);
                    frontmassData.Obj = Instantiate(_tJunctionPrefab, frontPos, Quaternion.Euler(0, rot3, 0), _parent);
                    break;
                // 直線もしくは角
                case 2:
                    if (roomDir == frontmassData.Dir)
                    {
                        float rot2 = dungeonCalc.GetPassStraightRotY(roomDir);
                        frontmassData.Obj = Instantiate(_passPrefab, frontPos, Quaternion.Euler(0, rot2, 0), _parent);
                    }
                    else
                    {
                        float rot2 = dungeonCalc.GetCornerRotY(roomDir, frontmassData.Dir);
                        frontmassData.Obj = Instantiate(_cornerPrefab, frontPos, Quaternion.Euler(0, rot2, 0), _parent);
                    }
                    break;
            }
        }
    }

    /// <param name="isPositive">trueだと前右後左の時計回り、falseだと反時計回り</param>
    Vector3Int RotDirVec90(Vector3Int dirVec, bool isPositive)
    {
        if (dirVec == Vector3Int.forward)
        {
            if (isPositive) return Vector3Int.right;
            else            return Vector3Int.left;
        }
        else if (dirVec == Vector3Int.right)
        {
            if (isPositive) return Vector3Int.back;
            else            return Vector3Int.forward;
        }
        else if (dirVec == Vector3Int.back)
        {
            if (isPositive) return Vector3Int.left;
            else            return Vector3Int.right;
        }
        else if (dirVec == Vector3Int.left)
        {
            if (isPositive) return Vector3Int.forward;
            else            return Vector3Int.back;
        }
        else
        {
            Debug.LogError("上下左右以外の角度です。: " + dirVec);
            return Vector3Int.zero;
        }
    }
}
