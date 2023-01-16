using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonHelper.Direction;

/// <summary>
/// 文字列に対応したダンジョンの通路を建てるコンポーネント
/// </summary>
public class DungeonPassBuilder : MonoBehaviour
{
    enum TurtleCommand
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
    //[SerializeField] GameObject _test;

    DungeonHelper _helper;
    Dictionary<Vector3Int, GameObject> _passDic;
    /// <summary生成後に見た目を修正するために条件を満たした通路を保持しておく</summary>
    HashSet<Vector3Int> _fixPassSet;

    void Awake()
    {
        _helper = new DungeonHelper();
        _passDic = new Dictionary<Vector3Int, GameObject>(PassDicCap);
        _fixPassSet = new HashSet<Vector3Int>(EdgePassSetCap);
    }

    /// <summary>全ての生成した通路のマスの座標を取得する</summary>
    internal IReadOnlyCollection<Vector3Int> GetPassPosAll() => _passDic.Keys;

    /// <summary>文字列をダンジョンの部品プレハブに変換する</summary>
    internal void ConvertToGameObject(string str)
    {
        // セーブ/ロードのコマンド用
        Stack<TurtleParam> saveStack = new Stack<TurtleParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int dir = Vector3Int.forward;
        int dist = MaxPassDist;
        
        foreach (char command in str)
        {
            switch ((TurtleCommand)command)
            {
                case TurtleCommand.Forward:
                    GeneratePass(currentPos, dir, dist);
                    currentPos = currentPos + dir * dist * _helper.PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                case TurtleCommand.RotRight:
                    dir = Rotate(dir, isPositive: true);
                    break;
                case TurtleCommand.RotLeft:
                    dir = Rotate(dir, isPositive: false);
                    break;
                case TurtleCommand.Save:
                    saveStack.Push(new TurtleParam(currentPos, dir, dist));
                    //Debug.Log($"Push:{currentPos},{dir},{dist}");
                    break;
                case TurtleCommand.Load:
                    if (saveStack.Count == 0) break;
                    TurtleParam param = saveStack.Pop();
                    currentPos = param.Pos;
                    dir        = param.Dir;
                    dist       = param.Dist;
                    //Debug.Log($"Pop:{currentPos},{dir},{dist}");
                    break;
            }
        }

        FixPass();
    }

    /// <summary>直線の通路を生成する</summary>
    void GeneratePass(Vector3Int startPos, Vector3Int dir, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Vector3Int pos = startPos + dir * i * _helper.PrefabScale;
            // 同じ座標に生成しないようにチェック
            if (_passDic.ContainsKey(pos)) continue;

            Quaternion rot = Quaternion.identity;
            if (dir == Vector3Int.right || dir == Vector3Int.left) rot = Quaternion.Euler(0, 90, 0);

            GameObject go = Instantiate(_passPrefab, pos, rot, _parent);
            // 生成した通路を弄るために辞書に追加しておく
            _passDic.Add(pos, go);

            bool require = i / 2 == 1;
            // 条件で絞ったマスと始点と終点を専用のコレクションに追加する
            // 条件を消すと精度が上がるが処理負荷も跳ね上がる
            if (require || i == 0 || i == dist - 1)
                _fixPassSet.Add(pos);
        }
    }

    /// <summary>通路を違和感のない見た目に修正する</summary>
    void FixPass()
    {
        foreach(Vector3Int pos in _fixPassSet)
        {
            // その座標が前後左右どの方向に接続されているか、いくつ接続されているか
            (int dirs, int count) = _helper.GetNeighbourInt(pos, _passDic.Keys);
            bool dirForward = (dirs & _helper.BForward) == _helper.BForward;
            bool dirBack =    (dirs & _helper.BBack)    == _helper.BBack;
            bool dirLeft =    (dirs & _helper.BLeft)    == _helper.BLeft;
            bool dirRight =   (dirs & _helper.BRight)   == _helper.BRight;

            Quaternion rot = Quaternion.identity;
            GameObject go = null;
            switch (count)
            {
                // 行き止まり
                case 1:
                    if      (dirBack)  rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirRight) rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirLeft)  rot.eulerAngles = new Vector3(0, -90, 0);

                    go = Instantiate(_passEndPrefab, pos, rot, _parent);
                    break;
                // 角
                case 2:
                    // 上下もしくは左右に接続されている場合は通路なので何もしない
                    if ((dirForward && dirBack) || (dirRight && dirLeft)) 
                        continue;

                    if      (dirForward && dirRight) rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirLeft && dirForward)  rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirRight && dirBack)    rot.eulerAngles = new Vector3(0, -90, 0);

                    go = Instantiate(_cornerPrefab, pos, rot, _parent);
                    break;
                // 丁字路
                case 3:
                    if      (dirForward && dirBack && dirLeft)  rot.eulerAngles = new Vector3(0, 180, 0);
                    else if (dirBack && dirRight && dirLeft)    rot.eulerAngles = new Vector3(0, 90, 0);
                    else if (dirForward && dirRight && dirLeft) rot.eulerAngles = new Vector3(0, -90, 0);

                    go = Instantiate(_tJunctionPrefab, pos, rot, _parent);
                    break;
                // 十字路
                case 4:
                    go = Instantiate(_crossPrefab, pos, rot, _parent);
                    break;
            }

            // 置き換えるので元あったオブジェクトは削除する
            Destroy(_passDic[pos]);

            // TODO:応急処置的に追加しているのでよく検討する
            _passDic[pos] = go;
        }
    }

    internal void FixConnectRoomEntrance(IReadOnlyDictionary<Vector3Int, Direction> dic)
    {
        // 通路に対して部屋が接続されると通路の接続数が+1される
        // ある通路に対して反対側からも部屋が接続される場合がある
        foreach (var v in dic)
        {
            GameObject go = _passDic[v.Key - GetSidePos(v.Value)];

            // TODO:名前で判定して処理を分岐する、テストなので別の方法を検討する必要あり
            //      反対側からの接続が考慮されていないので直す
            //      応じた回転を行う
            switch (go.name)
            {
                case "Dungeon_Pass(Clone)":
                    Instantiate(_tJunctionPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    Destroy(go);
                    break;
                case "Dungeon_PassEnd(Clone)":
                    // 正面の場合と左右に生成された場合で分岐しないといけない
                    if ((v.Value == Direction.Forward &&  Hoge(go) == Direction.Back) ||
                        (v.Value == Direction.Back && Hoge(go) == Direction.Forward) ||
                        (v.Value == Direction.Left && Hoge(go) == Direction.Right) ||
                        (v.Value == Direction.Right && Hoge(go) == Direction.Left))
                    {
                        Instantiate(_passPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(_cornerPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    }
                    Destroy(go);
                    break;
                case "Dungeon_Corner(Clone)":
                    Instantiate(_tJunctionPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    Destroy(go);
                    break;
                case "Dungeon_TJunction(Clone)":
                    Instantiate(_crossPrefab, v.Key - GetSidePos(v.Value), Quaternion.identity);
                    Destroy(go);
                    break;
            }
            //Instantiate(_test, v.Key - GetSidePos(v.Value), Quaternion.identity);
        }

        Direction Hoge(GameObject go)
        {
            Debug.Log(go.transform.rotation.y);
            if (go.transform.rotation.y == 0)
            {
                return Direction.Forward; 
            }
            if (go.transform.rotation.y == 0.7071068f)
            {
                return Direction.Right;
            }
            if (go.transform.rotation.y == -0.7071068f)
            {
                return Direction.Left;
            }
            if (go.transform.rotation.y == 1)
            {
                return Direction.Back;
            }

            Debug.LogError("なんか変");
            return Direction.Forward;
        }
    }

    // TODO:DungeonRoomBuilderにも同じメソッドがあるのでリファクタリングする
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

    /// <summary>前後左右の方向を回転させる</summary>
    /// <param name="isPositive">trueだと前右後左の時計回り、falseだと反時計回り</param>
    Vector3Int Rotate(Vector3Int currentDir, bool isPositive)
    {
        if (currentDir == Vector3Int.forward)
        {
            if (isPositive) return Vector3Int.right;
            else            return Vector3Int.left;
        }
        else if (currentDir == Vector3Int.right)
        {
            if (isPositive) return Vector3Int.back;
            else            return Vector3Int.forward;
        }
        else if (currentDir == Vector3Int.back)
        {
            if (isPositive) return Vector3Int.left;
            else            return Vector3Int.right;
        }
        else if (currentDir == Vector3Int.left)
        {
            if (isPositive) return Vector3Int.forward;
            else            return Vector3Int.back;
        }
        else
        {
            Debug.LogError("上下左右以外の角度です。: " + currentDir);
            return Vector3Int.zero;
        }
    }
}
