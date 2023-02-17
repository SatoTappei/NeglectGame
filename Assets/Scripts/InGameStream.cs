using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// �Q�[���S�̗̂���𐧌䂷��R���|�[�l���g
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] DungeonBuilder _dungeonBuilder;
    [SerializeField] InGameTimer _inGameTimer;
    [SerializeField] Generator _generator;

    async void Start()
    {
        // �J�����̓N�H�[�^�[�r���[�ŌŒ�

        // �^�C�g�����
        // �{�^�����N���b�N�ŃX�^�[�g
        // �X�e�[�W�������o
        // �t�F�[�h���ă{�^���ƃ��S��������
        // ���o�I����Ƀ^�C�}�[�X�^�[�g
        // n�b�Ԋu�Ŗ`���҂��_���W�����ɂ���Ă���
        //  �K�i�̈ʒu�ɖ`���҂𐶐�
        //  �`���҂̓_���W���������낤�낷��
        // �v���C���[��3��ނ�㩂̂����ǂꂩ��I��ōD���ȂƂ����㩂�u����
        // 㩂͖`���҂Ƀ_���[�W��^����
        // n�b�o������Q�[���I�[�o�[

        // ���U���g
        // ���l�̖`���҂𑒂�����

        // �^�C�g���ɖ߂�

        // �K�v��UI
        // ����:�^�C�}�[
        // �E��:�������`���҂̐�(�X�R�A)
        // �E��:㩗p�̃{�^��3��
        // �E:�e�`���҂̃X�e�[�^�X�A�C�R��5��

        // �_���W�����������ɃA�j���[�V����������̂�Capacity�𑝂₵�Čx��������
        // �������ׂ����ɂȂ����ꍇ�̓A�j���[�V��������߂邱��
        DOTween.SetTweensCapacity(500, 50);

        // �L�����N�^�[�𐶐�����̂Ƀ_���W�����̒n�`��񂪕K�v�Ȃ̂�
        // ��Ƀ_���W�����𐶐�����K�v������B
        _dungeonBuilder.Build();

        // ������1�t���[���҂��Ȃ���Ray������ɔ��肵�Ȃ�
        await UniTask.Yield();

        // �L�����N�^�[�������ɂ̓O���b�h�̏�񂪕K�v�Ȃ̂Ő�ɐ�������K�v������
        _pathfindingGrid.GenerateGrid();

        // DungeonBuilder�Ő�������Waypoint���擾���Čo�H�T���Ɏg����悤�ɂ���
        _waypointManager.RegisterWaypoint();

        // �C���Q�[���̃^�C�}�[�Ɩ`���҂̐����͂��ݍ����Ă��Ȃ�
        // �C���Q�[���̃^�C�}�[�̃X�^�[�g�Ɠ����ɓG�̐������s��Generator���N������
        // Generator�͓Ǝ��̊Ԋu�Ő������Ă���
        _generator.GenerateRegularlyAsync(new CancellationTokenSource()).Forget();

        // �C���Q�[���̃^�C�}�[�̊J�n�̓��\�b�h�̌Ăяo���ōs����
        // �l�̉��Z��MessagePipe��p�������b�Z�[�W���O�ōs��
        await _inGameTimer.StartAsync(this.GetCancellationTokenOnDestroy());

    }
}
