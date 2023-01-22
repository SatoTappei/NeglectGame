using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 経路探索を管理するコンポーネント
/// </summary>
public class PathfindingManager : MonoBehaviour
{
    struct PathRequest
    {
        //Vector3 _startPos;
        //Vector3 _endPos;
        //UnityAction<Vector3[], bool> _callBack;

        public PathRequest(Vector3 startPos, Vector3 endPos, UnityAction<Vector3[], bool> callBack)
        {
            StartPos = startPos;
            EndPos = endPos;
            CallBack = callBack;
        }

        public Vector3 StartPos { get; }
        public Vector3 EndPos { get; }
        public UnityAction<Vector3[], bool> CallBack { get; }
    }
    
    [SerializeField]AStarPathfinding _aStarPathfinding;

    static PathfindingManager _instance;

    Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    PathRequest _currentPathRequest;
    // TODO:パスを処理中かの判定をするフラグ、いらなそう
    bool _isProcessingPath;

    void Awake()
    {
        _instance = this;
    }

    public static void RequestPath(Vector3 startPos, Vector3 endPos, UnityAction<Vector3[], bool> callBack)
    {
        // パスのスタート地点とゴール地点、ゴール地点に到達したときのコールバック
        PathRequest pathRequest = new PathRequest(startPos, endPos, callBack);
        // 順に処理するためにキューに追加する
        _instance._pathRequestQueue.Enqueue(pathRequest);
        // 次のパスを処理しようと試みる
        _instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!_isProcessingPath && _pathRequestQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessingPath = true;
            
            _aStarPathfinding.StartPathfinding(_currentPathRequest.StartPos, _currentPathRequest.EndPos);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        // 外部からこのクラスが保持している今のパスリクエストのコールバックを実行する
        _currentPathRequest.CallBack(path, success);
        _isProcessingPath = false;
        
        TryProcessNext();
    }
}
