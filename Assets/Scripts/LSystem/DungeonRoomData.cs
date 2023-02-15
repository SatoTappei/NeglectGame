using System;
using UnityEngine;

/// <summary>
/// ��������镔���̃f�[�^�̃N���X
/// </summary>
[Serializable]
internal class DungeonRoomData
{
    [Header("�����̃o���G�[�V����")]
    [SerializeField] GameObject[] _prefabs;
    [Header("�����̑傫��")]
    [SerializeField] int _width;
    [SerializeField] int _depth;
    [Header("�����̍ő吶����")]
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
