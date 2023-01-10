using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンジョン生成を制御するコンポーネント
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] DungeonPassBuilder _dungeonPassBuilder;
    [SerializeField] DungeonRoomBuilder _dungeonRoomBuilder;

    void Start()
    {
        // ダンジョン生成の基礎となる文字列を生成する
        string str = _lSystem.Generate();
        
        // 文字列からダンジョンの通路を生成する
        _dungeonPassBuilder.ConvertToGameObject(str);
        var passColl = _dungeonPassBuilder.GetPassPosAll();
        
        // 通路に隣接した箇所に部屋を生成する
        _dungeonRoomBuilder.GenerateRoom(passColl);
    }

    void Update()
    {
        
    }
}
