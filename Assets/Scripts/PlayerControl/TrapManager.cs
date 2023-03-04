using UnityEngine;

/// <summary>
/// 罠の管理をするコンポーネント
/// </summary>
public class TrapManager : MonoBehaviour
{
    class TrapInstanceData
    {
        GameObject _instance;
        Vector3 _actualPos;

        public TrapInstanceData(GameObject instance, Vector3 actualPos)
        {
            _instance = instance;
            _actualPos = actualPos;
        }

        public Vector3 ActualPos { get => _actualPos; set => _actualPos = value; }
        public GameObject Instance { get => _instance; }
    }

    static readonly int TrapQuantity = 4;

    [Header("設置する罠のプレハブ")]
    [SerializeField] Trap _prefab;
    [Header("生成した罠を登録しておく親")]
    [SerializeField] Transform _trapParent;

    int _trapIndex;
    TrapInstanceData[] _trapPool = new TrapInstanceData[TrapQuantity];

    /// <summary>ありえない値の位置に生成する</summary>
    Vector3 TrapInitPos => Vector3.one * 999;

    public void Init()
    {
        for (int i = 0; i < TrapQuantity; i++)
        {
            Trap trap = Instantiate(_prefab, TrapInitPos, Quaternion.identity, _trapParent);
            _trapPool[i] = new TrapInstanceData(trap.gameObject, TrapInitPos);
        }
    }

    public GameObject TryGetTrap(Vector3 estimatePos)
    {
        if (_trapPool == null) return null;

        // 高々4つ程度なので線形探索で大丈夫
        foreach (TrapInstanceData t in _trapPool)
        {
            if (t.ActualPos == estimatePos) return null;
        }

        TrapInstanceData trap = _trapPool[_trapIndex++ % TrapQuantity];
        trap.ActualPos = estimatePos;

        return trap.Instance;
    }
}
