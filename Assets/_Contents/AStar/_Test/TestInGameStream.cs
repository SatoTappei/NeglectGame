using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
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

    void Start()
    {
        //await UniTask.WaitUntil(()=>Input.GetKeyDown(KeyCode.Space));
        // ���o��҂��߂�1�t���[��await����
        Hoge(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid Hoge(CancellationToken token)
    {
        await UniTask.Yield();
        await _actorGenerator.GenerateRegularly(token);
    }
}
