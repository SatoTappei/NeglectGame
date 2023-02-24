using UniRx;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generator�Ő��������I�u�W�F�N�g�𐶐������^�C�~���O��
/// �Q�Ƃ������R���|�[�l���g���g���ď������������肷��
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;
    //[Header("�Q�Ƃ��������������R���|�[�l���g�ւ̎Q��")]
    [SerializeField] WaypointManager _waypointManager;

    void Awake()
    {
        // Awake()��Enabled()�̌�AStart()�̑O�ɌĂ΂��
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject != null).Subscribe(gameObject =>
        {
            // Waypoint�𐶐��������Generator�R���|�[�l���g�������������Ȃ��Ƃ����Ȃ�
            // ���ԓI���������Ă���̂ŌĂяo�����ɒ���
            List<Vector3> list = _waypointManager.GetWaypointListWithWaypointType(WaypointType.Exit);
            int r = Random.Range(0, list.Count);
            gameObject.transform.position = list[r];

            // UI�ɏ��������n��
        });
    }
}
