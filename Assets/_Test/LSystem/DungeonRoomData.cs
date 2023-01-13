using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ��������镔���̃f�[�^�̃N���X
/// </summary>
[Serializable]
internal class DungeonRoomData
{
    [SerializeField] GameObject[] _prefabArr;
    [SerializeField] int _size;
    [SerializeField] int _maxQuantity;
    int _currentQuantity;

    internal int MaxQuantity => _maxQuantity;
    internal int Size => _size;

    internal GameObject GetPrefab()
    {
        // �������𐔂��邽�߂ɃJ�E���g�𑝂₷
        _currentQuantity++;

        int r = UnityEngine.Random.Range(0, _prefabArr.Length);
        return _prefabArr[r];
    }

    /// <summary>����ȏ㐶���\�����ׂ�</summary>
    internal bool CheckAvailable() => _currentQuantity < _maxQuantity;

    //internal void Reset() => _currentQuantity = 0;
}
