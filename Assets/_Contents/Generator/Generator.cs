using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using MessagePipe;
using VContainer;

// �`���҂�n�l�ȏ㐶�������Ȃ�
// ����I�Ƀt�B�[���h���̖`���Ґ����`�F�b�N����
// �����l���邩�̃t���O���K�v
// ����/�E�o/���S���ɏ㉺����

/// <summary>
/// ActorGenerator�ɐ������~/�ĊJ����悤���b�Z�[�W�̑���M����̂Ɏg���\����
/// </summary>
public struct GeneratorControl
{
    public bool IsGeneratable { get; }

    public GeneratorControl(bool isGeneratable)
    {
        IsGeneratable = isGeneratable;
    }
}

/// <summary>
/// �`���҂����I�ɐ�������W�F�l���[�^�̃R���|�[�l���g
/// </summary>
public class Generator : MonoBehaviour
{
    [Inject]
    readonly ISubscriber<GeneratorControl> _subscriber;

    [Header("��������v���n�u")]
    [SerializeField] GameObject[] _prefabs;
    [Header("��������Ԋu")]
    [SerializeField] float _interval = 1.0f;

    bool _isGeneratable = true;
    /// <summary>
    /// �O������L�����Z���p�̃��\�b�h���ĂԂ����Ő����������L�����Z���ł���悤��
    /// CancellationTokenSource�������瑤�ŕێ����Ă���
    /// </summary>
    CancellationTokenSource _tokenSource;

    void Start()
    {
        // �O������GeneratorControl��Publish���邱�ƂŐ����̒�~/�ĊJ���R���g���[������
        _subscriber.Subscribe(control => 
        {
            _isGeneratable = control.IsGeneratable;
        }).AddTo(this);
    }

    public async UniTask GenerateRegularlyAsync(CancellationTokenSource tokenSource)
    {
        _tokenSource = tokenSource;

        try
        {
            while (true)
            {
                // �C���^�[�o�����ɐ����\�t���O�������Ă����̐����^�C�~���O������܂ł͐�������Ȃ��B
                if (_isGeneratable)
                {
                    int r = UnityEngine.Random.Range(0, _prefabs.Length);

                    // TODO:�������ꂽ�ۂɏ�����������
                    // ��������ꏊ���K�i�ɂ�����
                    // ����������UI���V���b�Əo�����o��������
                    Instantiate(_prefabs[r]);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: tokenSource.Token);
            }
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("�����������L�����Z������܂����B: " + e.Message);
        }
    }

    public void GenerateRegularlyCancel() => _tokenSource?.Cancel();

    void OnDestroy()
    {
        _tokenSource.Cancel();
    }
}
