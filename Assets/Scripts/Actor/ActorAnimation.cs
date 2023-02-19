using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// キャラクターをアニメーションさせるコンポーネント
/// </summary>
public class ActorAnimation : MonoBehaviour
{
    /// <summary>
    /// ステート名に対応したAnimationClipの長さを取得するための構造体
    /// </summary>
    struct StateData
    {
        public StateData(string stateName, float clipLength)
        {
            Hash = Animator.StringToHash(stateName);
            Length = clipLength;
        }

        public int Hash { get; }
        public float Length { get; }
    }

    [SerializeField] Animator _anim;

    /// <summary>外部からステート名を指定してアニメーションを再生させるのに使用する</summary>
    Dictionary<string, StateData> _stateDataDic;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        // ステート名でAnimationClipの長さを取得できないので専用の構造体と辞書を用意
        AnimatorController controller = _anim.runtimeAnimatorController as AnimatorController;
        AnimationClip[] clips = controller.animationClips;
        ChildAnimatorState[] states = controller.layers[0].stateMachine.states;

        _stateDataDic = new Dictionary<string, StateData>(clips.Length);
        for (int i = 0; i < clips.Length; i++)
        {
            _stateDataDic.Add(states[i].state.name, new StateData(states[i].state.name, clips[i].length));
        }
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
            sequence.Append(_anim.gameObject.transform.DOLocalMove(Vector3.zero, 0.15f))
                    .AppendCallback(() =>
                    {
                        _anim.Play(stateData.Hash);
                        DOVirtual.DelayedCall(stateData.Length, () => callback?.Invoke());
                    })
                    .SetLink(gameObject);
        }
        else
        {
            Debug.LogError("対応するステートが存在しません:" + stateName);
        }
    }
}
