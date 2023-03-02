using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスを表示するUIを管理するコンポーネント
/// </summary>
public class ActorStatusUIManager : MonoBehaviour
{
    static readonly int InstantiateMax = 5;
    static readonly int InstantiateOffsetY = -100;

    [Header("生成するUIのPrefab")]
    [SerializeField] ActorStatusUI _prefab;
    [Header("生成したUIを登録する親")]
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

    /// <summary>外部からこのメソッドを呼ぶことでUIへの表示を行う</summary>
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
            Debug.LogError("キャラクターのステータスを表示するパネルが足りません");
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
