using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �_���W���������𐧌䂷��R���|�[�l���g
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] DungeonPassBuilder _dungeonPassBuilder;
    [SerializeField] DungeonRoomBuilder _dungeonRoomBuilder;

    /*
     �_���W���������̃��[��
        �����̕���3�ȏ�̊
        �����̉��s��3�ȏ�̐�
     */

    void Start()
    {
        // �_���W�����������ɃA�j���[�V����������̂�Capacity�𑝂₵�Čx��������
        // �������ׂ����ɂȂ����ꍇ�̓A�j���[�V��������߂邱��
        DOTween.SetTweensCapacity(500, 50);

        string result = _lSystem.Generate();

        _dungeonPassBuilder.BuildDungeonPass(result);
        var massDataAll = _dungeonPassBuilder.GetMassDataAll();

        _dungeonRoomBuilder.BuildDungeonRoom(massDataAll);
        var roomEntranceDic = _dungeonRoomBuilder.GetRoomEntranceDataAll();

        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);
    }
}
