using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // �_���W���������̊�b�ƂȂ镶����𐶐�����
        string str = _lSystem.Generate();
        
        // �����񂩂�_���W�����̒ʘH�𐶐�����
        _dungeonPassBuilder.ConvertToGameObject(str);
        var passColl = _dungeonPassBuilder.GetPassPosAll();
        
        // �ʘH�ɗאڂ����ӏ��ɕ����𐶐�����
        _dungeonRoomBuilder.GenerateRoom(passColl);
    }

    void Update()
    {
        
    }
}
