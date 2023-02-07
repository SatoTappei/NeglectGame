using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �Q�[���S�̗̂�����Ǘ�����R���|�[�l���g(�e�X�g�p)
/// </summary>
public class TestInGameStream : MonoBehaviour
{
    /* 
     *  �X�e�[�W�̎������������Ȃ���Ԃł�����Ɠ����悤�ɍ�� 
     */

    [SerializeField] ActorGenerator _actorGenerator;

    async void Start()
    {
        // ���O�ɉ��o�����݂����̂�Space���������܂�await
        await UniTask.WaitUntil(()=>Input.GetKeyDown(KeyCode.Space));
        await _actorGenerator.GenerateRegularly(this.GetCancellationTokenOnDestroy());
    }
}
