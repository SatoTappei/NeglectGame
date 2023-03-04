using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// �Q�[���S�̗̂���𐧌䂷��R���|�[�l���g
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] TitleUIControl _titleUIControl;
    [SerializeField] ResultUIControl _resultUIControl;
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] DungeonBuilder _dungeonBuilder;
    [SerializeField] TrapManager _trapManager;
    [SerializeField] InGameTimer _inGameTimer;
    [SerializeField] Generator _generator;

    void Start()
    {
        Stream(this.GetCancellationTokenOnDestroy()).Forget();
    }

    async UniTaskVoid Stream(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // �_���W�����������ɃA�j���[�V����������̂�Capacity�𑝂₵�Čx��������
        // �������ׂ����ɂȂ����ꍇ�̓A�j���[�V��������߂邱��
        DOTween.SetTweensCapacity(500, 50);

        // �^�C�g����ʂ���J�ڂ���̂�҂�
        await _titleUIControl.TitleStateAsync(token);

        AudioManager.Instance.PlayBGM("BGM_�C���Q�[��");

        // �L�����N�^�[�𐶐�����̂Ƀ_���W�����̒n�`��񂪕K�v�Ȃ̂�
        // ��Ƀ_���W�����𐶐�����K�v������B
        _dungeonBuilder.Build();

        // �����Ń_���W�����������̃A�j���[�V�������҂��Ȃ���Ray���������Ȃ�
        // ���Q�[���X�^�[�g�I�̉��o�����
        await UniTask.Delay(System.TimeSpan.FromSeconds(1.0f));

        // �L�����N�^�[�������ɂ̓O���b�h�̏�񂪕K�v�Ȃ̂Ő�ɐ�������K�v������
        _pathfindingGrid.GenerateGrid();

        // DungeonBuilder�Ő�������Waypoint���擾���Čo�H�T���Ɏg����悤�ɂ���
        _waypointManager.RegisterWaypoint();

        // �C���Q�[���̃^�C�}�[�Ɩ`���҂̐����͂��ݍ����Ă��Ȃ�
        // �C���Q�[���̃^�C�}�[�̃X�^�[�g�Ɠ����ɓG�̐������s��Generator���N������
        // Generator�͓Ǝ��̊Ԋu�Ő������Ă���
        CancellationTokenSource cts = new();
        _generator.GenerateRegularlyAsync(cts).Forget();

        _trapManager.Init();

        // �C���Q�[���̃^�C�}�[�̊J�n�̓��\�b�h�̌Ăяo���ōs����
        // �l�̉��Z��MessagePipe��p�������b�Z�[�W���O�ōs��
        await _inGameTimer.StartAsync(token);
        cts.Cancel();

        await _resultUIControl.AnimationAsync(token);
    }
}
