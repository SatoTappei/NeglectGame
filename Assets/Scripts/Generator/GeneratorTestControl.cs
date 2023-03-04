using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePipe;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// �`���҂𐶐�����W�F�l���[�^�̐�����e�X�g����R���|�[�l���g
/// </summary>
public class GeneratorTestControl : MonoBehaviour
{
    [Inject]
    readonly IPublisher<GeneratorControl> _publisher;

    [SerializeField] Generator _generator;

    void Start()
    {
        // �W�F�l���[�^�̋N���̓R���|�[�l���g�̎Q�Ƃ��ă��\�b�h�Ăяo���ōs����
        // �����̒�~/�ĊJ�̓��b�Z�[�W����čs��
        _generator.GenerateRegularlyAsync(new CancellationTokenSource()).Forget();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: false));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _publisher.Publish(new GeneratorControl(isGeneratable: true));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //_generator.GenerateRegularlyCancel();
        }
    }
}
