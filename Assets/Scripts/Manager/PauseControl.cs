using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �|�[�Y�@�\�𐧌䂷��R���|�[�l���g
/// </summary>
public class PauseControl : MonoBehaviour
{
    /// <summary>�����e�ʂɃL�����N�^�[�̐����̍ő吔���𑫂�</summary>
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
                Debug.LogWarning("IPauseable����������Ă��܂���" + pauseObject.name);
                continue;
            }

            _pauseableList.AddRange(pauseables);
        }
    }

    public void Add(GameObject pauseableObject) => PauseableListControl(pauseableObject, _pauseableList.Add);
    public void Remove(GameObject pauseableObject) => PauseableListControl(pauseableObject, ListRemove);

    /// <summary>Remove()��bool��Ԃ��̂�PauseableListControl()�ň�����悤�Ƀ��b�v����</summary>
    void ListRemove(IPauseable pauseable) => _pauseableList.Remove(pauseable);

    void PauseableListControl(GameObject pauseableObject, UnityAction<IPauseable> listControl)
    {
        IPauseable[] pauseables = pauseableObject.GetComponents<IPauseable>();
        if (pauseables == null)
        {
            Debug.LogWarning("IPauseable����������Ă��܂���" + pauseableObject.name);
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
