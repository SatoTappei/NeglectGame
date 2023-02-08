using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// �`���҂𐶐�����R���|�[�l���g
/// </summary>
public class ActorGenerator : MonoBehaviour
{
    // �ő�3�l�ȏ�̓t�B�[���h�ɐ��������Ȃ�
    // �`���҂����������ƃX�e�[�^�X��\��UI���V���b�Ɖ�����o�Ă���
    [SerializeField] GameObject _prefab;

    // �e�X�g�p�̐����\�t���O
    // ��ʂ���I�������t���O�͕ʂ̏ꏊ�ɊǗ�����悤�ɂ���
    bool _isGeneratable;

    internal async UniTask GenerateRegularly(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        while (true)
        {
            GenerateActor();
            // �e�X�g�p��G�L�[���������琶������悤�ɂ��Ă���
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.G), cancellationToken: token);
        }
    }

    void GenerateActor()
    {
        Instantiate(_prefab, transform.position, Quaternion.identity);
        // �����ɐ������̉��o������
    }
}
