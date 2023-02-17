using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// Generator�Ő��������I�u�W�F�N�g�𐶐������^�C�~���O��
/// �Q�Ƃ������R���|�[�l���g���g���ď������������肷��
/// </summary>
public class GenerateObserver : MonoBehaviour
{
    [SerializeField] Generator _generator;
    [Header("�Q�Ƃ��������������R���|�[�l���g�ւ̎Q��")]
    [SerializeField] WaypointManager _waypointManager;

    void Awake()
    {
        // Awake()��Enabled()�̌�AStart()�̑O�ɌĂ΂��
        _generator.LastInstantiatedPrefab.Where(gameobject => gameobject != null).Subscribe(gameObject =>
        {
            // �o�H�T���Ɏg��Waypoint��n��
            IReadOnlyDictionary<WaypointType, List<Vector3>> dic = _waypointManager.WaypointDic;
            gameObject.GetComponent<ActorPathfindingWaypoint>().Init(dic);

            // �ʒu���K�i�ɂ���

            // UI�ɏ��������n��
        });
    }
}
