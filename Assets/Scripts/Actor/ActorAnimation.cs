using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �L�����N�^�[���A�j���[�V����������R���|�[�l���g
/// </summary>
public class ActorAnimation : MonoBehaviour
{
    /// <summary>
    /// �X�e�[�g���ɑΉ�����AnimationClip�̒������擾���邽�߂̍\����
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

    /// <summary>�O������X�e�[�g�����w�肵�ăA�j���[�V�������Đ�������̂Ɏg�p����</summary>
    Dictionary<string, StateData> _stateDataDic;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        // �X�e�[�g����AnimationClip�̒������擾�ł��Ȃ��̂Ő�p�̍\���̂Ǝ�����p��
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
            Debug.LogError("�Ή�����X�e�[�g�����݂��܂���:" + stateName);
        }
    }
}
