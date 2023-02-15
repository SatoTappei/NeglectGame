using System;
using UnityEngine;

/// <summary>
/// 生成される部屋のデータのクラス
/// </summary>
[Serializable]
internal class DungeonRoomData
{
    [Header("部屋のバリエーション")]
    [SerializeField] GameObject[] _prefabs;
    [Header("部屋の大きさ")]
    [SerializeField] int _width;
    [SerializeField] int _depth;
    [Header("部屋の最大生成数")]
    [SerializeField] int _maxQuantity;

    int _currentQuantity;

    internal int MaxQuantity => _maxQuantity;
    internal int Width => _width;
    internal int Depth => _depth;

    internal GameObject GetRandomVariationPrefab()
    {
        _currentQuantity++;

        int r = UnityEngine.Random.Range(0, _prefabs.Length);
        return _prefabs[r];
    }

    internal bool IsAvailable() => _currentQuantity < _maxQuantity;
}
