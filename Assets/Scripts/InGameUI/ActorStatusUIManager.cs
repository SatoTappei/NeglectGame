using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̃X�e�[�^�X��\������UI���Ǘ�����R���|�[�l���g
/// </summary>
public class ActorStatusUIManager : MonoBehaviour
{
    static readonly int InstantiateMax = 5;
    static readonly int InstantiateOffsetY = -100;

    [Header("��������UI��Prefab")]
    [SerializeField] ActorStatusUI _prefab;
    [Header("��������UI��o�^����e")]
    [SerializeField] Transform _parent;

    Vector3 InitUIPos => new Vector3(-300, 200, 0);

    Queue<ActorStatusUI> _unUsedqueue = new(InstantiateMax);
    Queue<ActorStatusUI> _usedqueue = new(InstantiateMax);

    void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    public void Init()
    {
        for(int i = 1; i <= InstantiateMax; i++)
        {
            Vector3 pos = InitUIPos;
            pos.y = pos.y * i + InstantiateOffsetY;
            ActorStatusUI instance = Instantiate(_prefab, pos, Quaternion.identity, _parent);
            _unUsedqueue.Enqueue(instance);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I))
        {

        }
    }

    public ActorStatusUI GetUnUsedUI()
    {
        if (_unUsedqueue.Count == 0)
        {
            Debug.LogError("�L�����N�^�[�̃X�e�[�^�X��\������p�l��������܂���");
            return null;
        }
        else
        {
            ActorStatusUI ui = _unUsedqueue.Dequeue();
            _usedqueue.Enqueue(ui);
            return ui;
        }

    }
}
