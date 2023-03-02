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
    List<ActorStatusUI> _usedList = new(InstantiateMax);

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        for(int i = 1; i <= InstantiateMax; i++)
        {
            Vector3 pos = InitUIPos;
            pos.y = pos.y * i + InstantiateOffsetY;
            ActorStatusUI instance = Instantiate(_prefab, pos, Quaternion.identity, _parent);
            instance.Init(this);

            _unUsedqueue.Enqueue(instance);
        }
    }

    /// <summary>�O�����炱�̃��\�b�h���ĂԂ��Ƃ�UI�ւ̕\�����s��</summary>
    public ActorStatusUI GetNewActiveUI(Sprite icon, int maxHp)
    {
        ActorStatusUI ui = GetUnUsedUI();
        ui.SetValueAll(icon, maxHp);
        ui.Play();

        return ui;
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
            _usedList.Add(ui);
            return ui;
        }
    }

    public void ReturnUI(ActorStatusUI ui)
    {
        _usedList.Remove(ui);
        _unUsedqueue.Enqueue(ui);
    }
}
