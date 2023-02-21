using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using MessagePipe;
using VContainer;

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
    [Header("���������v���n�u��o�^����e")]
    [SerializeField] Transform _parent;
    [Header("��������Ԋu")]
    [SerializeField] float _interval = 1.0f;

    bool _isGeneratable = true;
    /// <summary>
    /// �O������L�����Z���p�̃��\�b�h���ĂԂ����Ő����������L�����Z���ł���悤��
    /// CancellationTokenSource�������瑤�ŕێ����Ă���
    /// </summary>
    CancellationTokenSource _cts;
    /// <summary>
    /// �I�u�W�F�N�g�𐶐������^�C�~���O��
    /// �O�����炻�̃I�u�W�F�N�g���������ł���悤�ɕێ����Ă���
    /// </summary>
    ReactiveProperty<GameObject> _lastInstantiatedPrefab = new ();

    public IReadOnlyReactiveProperty<GameObject> LastInstantiatedPrefab => _lastInstantiatedPrefab;

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
        _cts = tokenSource;

        try
        {
            while (true)
            {
                // �C���^�[�o�����ɐ����\�t���O�������Ă����̐����^�C�~���O������܂ł͐�������Ȃ��B
                if (_isGeneratable)
                {
                    int r = UnityEngine.Random.Range(0, _prefabs.Length);
                    // ���������ۂ̏�����������ʂ̃R���|�[�l���g�ɈϔC����
                    _lastInstantiatedPrefab.Value = Instantiate(_prefabs[r], _parent);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_interval), cancellationToken: tokenSource.Token);
            }
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("�����������L�����Z������܂����B: " + e.Message);
        }
    }

    public void GenerateRegularlyCancel() => _cts?.Cancel();

    void OnDestroy()
    {
        _cts?.Cancel();
    }
}
