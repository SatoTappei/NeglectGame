using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 㩂̊Ǘ�������R���|�[�l���g
/// </summary>
public class TrapManager : MonoBehaviour
{
    class Trap
    {
        GameObject _instance;
        Vector3 _pos;

        public Trap(GameObject instance, Vector3 pos)
        {
            _instance = instance;
            _pos = pos;
        }

        public Vector3 Pos { get => _pos; set => _pos = value; }
        public GameObject Instance { get => _instance; }
    }

    static readonly int TrapQuantity = 4;

    [Header("�ݒu����㩂̃v���n�u")]
    [SerializeField] GameObject _prefab;
    [Header("��������㩂�o�^���Ă����e")]
    [SerializeField] Transform _trapParent;

    int _trapIndex;
    Trap[] _trapPool = new Trap[TrapQuantity];

    /// <summary>���肦�Ȃ��l�̈ʒu�ɐ�������</summary>
    Vector3 TrapInitPos => Vector3.one * 999;

    public void Init()
    {
        for (int i = 0; i < TrapQuantity; i++)
        {
            GameObject instance = Instantiate(_prefab, TrapInitPos, Quaternion.identity, _trapParent);
            _trapPool[i] = new Trap(instance, TrapInitPos);
        }
    }

    public GameObject TryGetTrap(Vector3 pos)
    {
        foreach (Trap t in _trapPool)
        {
            Debug.Log(t.Pos + " " + pos);
            if (t.Pos == pos) return null;
        }

        Trap trap = _trapPool[_trapIndex % TrapQuantity];
        trap.Pos = pos;
        _trapIndex++;
        return trap.Instance;
    }
}
