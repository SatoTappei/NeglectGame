using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Direction = DungeonHelper.Direction;

/// <summary>
/// �_���W���������𐧌䂷��R���|�[�l���g
/// </summary>
public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] LSystem _lSystem;
    [SerializeField] DungeonPassBuilder _dungeonPassBuilder;
    [SerializeField] DungeonRoomBuilder _dungeonRoomBuilder;

    void Start()
    {
        // �_���W�����������ɃA�j���[�V����������̂�Capacity�𑝂₵�Čx��������
        // �������ׂ����ɂȂ����ꍇ�̓A�j���[�V��������߂邱��
        DOTween.SetTweensCapacity(500, 50);

        // �_���W���������̊�b�ƂȂ镶����𐶐�����
        string str = _lSystem.Generate();
        
        // �����񂩂�_���W�����̒ʘH�𐶐�����
        _dungeonPassBuilder.ConvertToGameObject(str);
        var passColl = _dungeonPassBuilder.GetPassPosAll();
        
        // �ʘH�ɗאڂ����ӏ��ɕ����𐶐�����
        _dungeonRoomBuilder.GenerateRoom(passColl);

        // �����ƂȂ���悤�ʘH���C������
        Dictionary<Vector3Int, Direction> roomEntranceDic = _dungeonRoomBuilder.RoomEntranceDic;
        _dungeonPassBuilder.FixConnectRoomEntrance(roomEntranceDic);
    }
}
