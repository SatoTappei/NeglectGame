using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ポーズ機能を制御するコンポーネント
/// </summary>
public class PauseControl : MonoBehaviour
{
    /// <summary>初期容量にキャラクターの生成の最大数分を足す</summary>
    static readonly int AddInitCap = 10;

    [SerializeField] GameObject[] _pauseObjects;

    List<IPauseable> _pauseableList;

    public void Init()
    {
        _pauseableList = new(_pauseObjects.Length + AddInitCap);

        foreach(GameObject pauseObject in _pauseObjects)
        {
            IPauseable[] pauseables = pauseObject.GetComponents<IPauseable>();
            if (pauseables == null)
            {
                Debug.LogWarning("IPauseableが実装されていません" + pauseObject.name);
                continue;
            }

            _pauseableList.AddRange(pauseables);
        }
    }

    public void Add(GameObject pauseableObject) => PauseableListControl(pauseableObject, _pauseableList.Add);
    public void Remove(GameObject pauseableObject) => PauseableListControl(pauseableObject, ListRemove);

    /// <summary>Remove()がboolを返すのでPauseableListControl()で扱えるようにラップする</summary>
    void ListRemove(IPauseable pauseable) => _pauseableList.Remove(pauseable);

    void PauseableListControl(GameObject pauseableObject, UnityAction<IPauseable> listControl)
    {
        IPauseable[] pauseables = pauseableObject.GetComponents<IPauseable>();
        if (pauseables == null)
        {
            Debug.LogWarning("IPauseableが実装されていません" + pauseableObject.name);
        }
        else
        {
            foreach(IPauseable pauseable in pauseables)
            {
                listControl(pauseable);
            }
        }
    }

    public void Pause() => _pauseableList.ForEach(p => p.Pause());
}
