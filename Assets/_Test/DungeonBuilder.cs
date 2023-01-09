using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 文字列に対応したダンジョンを建てる
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    enum TurtleCommand
    {
        Forward = 'F',
        RotRight = '+',
        RotLeft = '-',
        Save = '[',
        Load = ']',
    }

    //readonly int Angle = 90;
    readonly int PrefabScale = 3;
    readonly int DecreaseDist = 2;
    //readonly int MaxDist = 8;
    readonly int SaveStackCap = 4;

    // TODO:現在は仮のためここにLSystemの参照を持たせているが、後々移動させることを留意しておく
    [SerializeField] LSystem _lSystem;
    [Header("ダンジョンを構成する部品")]
    [SerializeField] GameObject _passPrefab;
    [Header("生成したダンジョンの部品の親")]
    [SerializeField] Transform _parent;

    List<Vector3Int> _posList = new List<Vector3Int>(10);
    //int _dist;

    void Start()
    {
        //_dist = MaxDist;
        Convert(_lSystem.Generate());
    }

    /// <summary>文字列をダンジョンの部品プレハブに変換する</summary>
    void Convert(string str)
    {
        // セーブ/ロードのコマンド用
        Stack<TurtleParam> saveStack = new Stack<TurtleParam>(SaveStackCap);

        Vector3Int currentPos = Vector3Int.zero;
        Vector3Int tempPos = Vector3Int.zero;
        Vector3Int dir = Vector3Int.forward;
        int dist = 8;

        _posList.Add(currentPos);
        
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
    }

    /// <summary>直線の通路を生成する</summary>
    void GeneratePass(Vector3Int startPos, Vector3Int dir, int dist)
    {
        for (int i = 0; i < dist; i++)
        {
            Instantiate(_passPrefab, startPos + dir * i * PrefabScale, Quaternion.identity, _parent);
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
