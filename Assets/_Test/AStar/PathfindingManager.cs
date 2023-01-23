using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �o�H�T�����Ǘ�����R���|�[�l���g
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
    // TODO:�p�X�����������̔��������t���O�A����Ȃ���
    bool _isProcessingPath;

    void Awake()
    {
        _instance = this;
    }

    public static void RequestPath(Vector3 startPos, Vector3 endPos, UnityAction<Vector3[], bool> callBack)
    {
        // �p�X�̃X�^�[�g�n�_�ƃS�[���n�_�A�S�[���n�_�ɓ��B�����Ƃ��̃R�[���o�b�N
        PathRequest pathRequest = new PathRequest(startPos, endPos, callBack);
        // ���ɏ������邽�߂ɃL���[�ɒǉ�����
        _instance._pathRequestQueue.Enqueue(pathRequest);
        // ���̃p�X���������悤�Ǝ��݂�
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
        // �O�����炱�̃N���X���ێ����Ă��鍡�̃p�X���N�G�X�g�̃R�[���o�b�N�����s����
        _currentPathRequest.CallBack(path, success);
        _isProcessingPath = false;
        
        TryProcessNext();
    }
}
