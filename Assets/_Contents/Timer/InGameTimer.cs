using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using VContainer;

/// <summary>
/// �C���Q�[�����Ɏg�p����^�C�}�[�ɒl�����Z���邽�߂Ɏg���\����
/// ���b�Z�[�W�̑���M�̂��߂�int�^�����b�v��������
/// </summary>
public struct InGameTimerAddValue
{
    public int Value { get; }

    public InGameTimerAddValue(int value)
    {
        Value = value;
    }
}

/// <summary>
/// �C���Q�[�����Ɏg�p����^�C�}�[�̃R���|�[�l���g
/// </summary>
public class InGameTimer : MonoBehaviour
{
    [Inject]
    readonly ISubscriber<InGameTimerAddValue> _subscriber;

    [Header("�C���Q�[���̐�������(�b)")]
    [SerializeField] int _timeLimit;

    ReactiveProperty<int> _count = new ReactiveProperty<int>();
    int _addValue;

    public IReadOnlyReactiveProperty<int> Count => _count;

    void Start()
    {
        //_subscriber.Subscribe(addValue => 
        //{
        //    AddCount(addValue.Value);
        //}).AddTo(this);
    }

    public async UniTask CountStart(CancellationToken token)
    {
        _count.Value = _timeLimit;

        try
        {
            await Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
                .Select(zeroStartValue =>
                {
                    token.ThrowIfCancellationRequested();

                    int current = (int)(_timeLimit - zeroStartValue);
                    _count.Value = current + _addValue;

                    return _count.Value;
                })
                .TakeWhile(i => i > 0);
        }
        catch(OperationCanceledException e)
        {
            UnityEngine.Debug.Log("�C���Q�[���̃^�C�}�[�̏������L�����Z������܂����B: " + e.Message);
        }  
    }

    // ���ۂ�UI�ɔ��f�����͎̂���OnNext()�����s���ꂽ�^�C�~���O
    void AddCount(int value) => _addValue += value;
}