using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// �L�����N�^�[���A�j���[�V����������R���|�[�l���g
/// </summary>
public class ActorAnimation : MonoBehaviour
{
    /// <summary>Animator�̑��X�e�[�g��</summary>
    static readonly int StateDataDicCap = 7;

    /// <summary>
    /// �X�e�[�g���ɑΉ������A�j���[�V�������Đ����邽�߂̃N���X
    /// </summary>
    [Serializable]
    class StateData
    {
        [SerializeField] string _name;
        [SerializeField] AnimationClip _clip;

        int _hash;
        float _length;

        public void Init(Dictionary<string, StateData> dic)
        {
            _hash = Animator.StringToHash(_name);
            _length = _clip.length;

            dic.Add(_name, this);
        }

        public int Hash => _hash;
        public float Length => _length;
    }

    [SerializeField] Animator _anim;
    [Header("�X�e�[�g�ɕR�Â����A�j���[�V����")]
    [SerializeField] StateData[] _stateDatas;

    Transform _animTransform;
    /// <summary>�O������X�e�[�g�����w�肵�ăA�j���[�V�������Đ�������̂Ɏg�p����</summary>
    Dictionary<string, StateData> _stateDataDic = new Dictionary<string, StateData>(StateDataDicCap);

    public void InitOnStart()
    {
        foreach (StateData data in _stateDatas)
        {
            data.Init(_stateDataDic);
        }

        _animTransform = _anim.gameObject.transform;
    }

    public float GetStateLength(string stateName)
    {
        if (_stateDataDic.TryGetValue(stateName, out StateData stateData))
        {
            return stateData.Length;
        }
        else
        {
            Debug.LogError("�X�e�[�g���o�^����Ă��܂���: " + stateName);
            return -1;
        }
    }

    internal void PlayAnim(string stateName)
    {
        if(_stateDataDic.TryGetValue(stateName, out StateData stateData))
        {
            _anim.Play(stateData.Hash);
        }     
        else
        {
            Debug.LogError("�Ή�����X�e�[�g�����݂��܂���: " + stateName);
        }
    }

    internal void PlayAnim(string stateName, UnityAction callback)
    {
        if (_stateDataDic.TryGetValue(stateName, out StateData stateData))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_animTransform.DOLocalMove(Vector3.zero, 0.15f))
                    .AppendCallback(() =>
                    {
                        _anim.Play(stateData.Hash);
                        DOVirtual.DelayedCall(stateData.Length, () => callback?.Invoke()).SetLink(gameObject);
                    })
                    .SetLink(gameObject);
        }
        else
        {
            Debug.LogError("�Ή�����X�e�[�g�����݂��܂���:" + stateName);
        }
    }
}
