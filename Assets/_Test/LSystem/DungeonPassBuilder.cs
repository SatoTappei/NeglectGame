using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = DungeonBuildingHelper.Direction;

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

    readonly int PrefabScale = 3;
    readonly int DecreaseDist = 2;
    readonly int SaveStackCap = 4;
    readonly int PassDicCap = 64;
    readonly int EdgePassSetCap = 16;

    // TODO:現在は仮のためここにLSystemの参照を持たせているが、後々移動させることを留意しておく
    [SerializeField] LSystem _lSystem;
    [Header("ダンジョンの通路を構成する部品")]
    [SerializeField] GameObject _passPrefab;
    [SerializeField] GameObject _cornerPrefab;
    [SerializeField] GameObject _tJunctionPrefab;
    [SerializeField] GameObject _crossPrefab;
    [SerializeField] GameObject _passEndPrefab;
    [Header("生成した部品の親")]
    [SerializeField] Transform _parent;

    //List<Vector3Int> _posList = new List<Vector3Int>(10);
    Dictionary<Vector3Int, GameObject> _passDic;
    /// <summary>各通路の両端を保持しておく</summary>
    HashSet<Vector3Int> _edgePassSet;

    void Start()
    {
        //_dist = MaxDist;
        _passDic = new Dictionary<Vector3Int, GameObject>(PassDicCap);
        _edgePassSet = new HashSet<Vector3Int>(EdgePassSetCap);
        ConvertToGameObject(_lSystem.Generate());
    }

    /// <summary>文字列をダンジョンの部品プレハブに変換する</summary>
    void ConvertToGameObject(string str)
    {
        // セーブ/ロードのコマンド用
        Stack<TurtleParam> saveStack = new Stack<TurtleParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        //Vector3Int tempPos = Vector3Int.zero;
        Vector3Int dir = Vector3Int.forward;
        int dist = 8;

        //_posList.Add(currentPos);
        
        foreach (char command in str)
        {
            switch ((TurtleCommand)command)
            {
                case TurtleCommand.Forward:
                    GeneratePass(currentPos, dir, dist);
                    currentPos = currentPos + dir * dist * PrefabScale;
                    dist -= DecreaseDist;
                    dist = Mathf.Max(1, dist);
                    break;
                case TurtleCommand.RotRight:
                    dir = RotDir(dir, isPositive: true);
                    break;
                case TurtleCommand.RotLeft:
                    dir = RotDir(dir, isPositive: false);
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
            Vector3Int pos = startPos + dir * i * PrefabScale;
            // 同じ座標に生成しないようにチェック
            if (_passDic.ContainsKey(pos)) continue;

            Quaternion rot = Quaternion.identity;
            if (dir == Vector3Int.right || dir == Vector3Int.left) rot = Quaternion.Euler(0, 90, 0);

            GameObject go = Instantiate(_passPrefab, pos, rot, _parent);
            // 生成した通路を弄るために辞書に追加しておく
            _passDic.Add(pos, go);

            // 始点と終点を専用のコレクションに追加する
            if (i == 0 || i == dist - 1)
                _edgePassSet.Add(pos);
        }
    }

    /// <summary>通路を違和感のない見た目に修正する</summary>
    void FixPass()
    {
        DungeonPassHelper helper = new DungeonPassHelper();
        // 通路の端の座標を渡してどの方向に接続されているか調べてもらう
        foreach(Vector3Int pos in _edgePassSet)
        {
            // 座標と通路の全座標が入ったCollectionを渡す
            HashSet<Direction> dirSet = helper.GetNeighbour(pos, _passDic.Keys);
            Quaternion rot = Quaternion.identity;

            switch (dirSet.Count)
            {
                // 行き止まり
                case 1:
                    if (dirSet.Contains(Direction.Back))    rot.eulerAngles = new Vector3(0, 180, 0);
                    if (dirSet.Contains(Direction.Right))   rot.eulerAngles = new Vector3(0, 90, 0);
                    if (dirSet.Contains(Direction.Left))    rot.eulerAngles = new Vector3(0, -90, 0);
                    Destroy(_passDic[pos]);
                    Instantiate(_passEndPrefab, pos, rot);
                    break;
                // 角
                case 2:
                    // 上下もしくは左右に接続されている場合は通路なので何もしない
                    if ((dirSet.Contains(Direction.Forward) && dirSet.Contains(Direction.Back)) ||
                        (dirSet.Contains(Direction.Right)   && dirSet.Contains(Direction.Left)))
                        continue;

                    if (dirSet.Contains(Direction.Forward) && dirSet.Contains(Direction.Right))
                        rot.eulerAngles = new Vector3(0, 180, 0);
                    if (dirSet.Contains(Direction.Left) && dirSet.Contains(Direction.Forward))
                        rot.eulerAngles = new Vector3(0, 90, 0);
                    if (dirSet.Contains(Direction.Right) && dirSet.Contains(Direction.Back))
                        rot.eulerAngles = new Vector3(0, -90, 0);

                    Destroy(_passDic[pos]);
                    Instantiate(_cornerPrefab, pos, rot);
                    break;
                // 丁字路
                case 3:
                    if (dirSet.Contains(Direction.Forward) && dirSet.Contains(Direction.Back) && dirSet.Contains(Direction.Left))
                        rot.eulerAngles = new Vector3(0, 180, 0);
                    if (dirSet.Contains(Direction.Back) && dirSet.Contains(Direction.Right) && dirSet.Contains(Direction.Left))
                        rot.eulerAngles = new Vector3(0, 90, 0);
                    if (dirSet.Contains(Direction.Forward) && dirSet.Contains(Direction.Right) && dirSet.Contains(Direction.Left))
                        rot.eulerAngles = new Vector3(0, -90, 0);

                    Destroy(_passDic[pos]);
                    Instantiate(_tJunctionPrefab, pos, rot);
                    break;
                // 十字路
                case 4:
                    Destroy(_passDic[pos]);
                    Instantiate(_crossPrefab, pos, rot);
                    break;
            }
        }
    }

    /// <summary>前後左右の方向を回転させる</summary>
    /// <param name="isPositive">trueだと前右後左の時計回り、falseだと反時計回り</param>
    Vector3Int RotDir(Vector3Int currentDir, bool isPositive)
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
