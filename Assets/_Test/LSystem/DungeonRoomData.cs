using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 生成される部屋のデータのクラス
/// </summary>
[Serializable]
internal class DungeonRoomData
{
    [SerializeField] GameObject[] _prefabArr;
    [SerializeField] int _width;
    [SerializeField] int _depth;
    [SerializeField] int _maxQuantity;
    int _currentQuantity;

    internal int MaxQuantity => _maxQuantity;
    internal int Width => _width;
    internal int Depth => _depth;

    internal GameObject GetPrefab()
    {
        // 生成数を数えるためにカウントを増やす
        _currentQuantity++;

        int r = UnityEngine.Random.Range(0, _prefabArr.Length);
        return _prefabArr[r];
    }

    /// <summary>これ以上生成可能か調べる</summary>
    internal bool IsAvailable() => _currentQuantity < _maxQuantity;

    //internal void Reset() => _currentQuantity = 0;
}
