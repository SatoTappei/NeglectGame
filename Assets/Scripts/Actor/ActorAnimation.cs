using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// キャラクターをアニメーションさせるコンポーネント
/// </summary>
public class ActorAnimation : MonoBehaviour
{
    /// <summary>Animatorの総ステート数</summary>
    static readonly int StateDataDicCap = 7;

    /// <summary>
    /// ステート名に対応したアニメーションを再生するためのクラス
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
    [Header("ステートに紐づいたアニメーション")]
    [SerializeField] StateData[] _stateDatas;

    Transform _animTransform;
    /// <summary>外部からステート名を指定してアニメーションを再生させるのに使用する</summary>
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
            Debug.LogError("ステートが登録されていません: " + stateName);
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
            Debug.LogError("対応するステートが存在しません: " + stateName);
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
            Debug.LogError("対応するステートが存在しません:" + stateName);
        }
    }
}
