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
    [Header("�Q�Ƃ��������������R���|�[�l���g�ւ̎Q��")]
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] ActorStatusUIManager _actorStatusUIManager;

    void Awake()
    {
        // Awake()��Enabled()�̌�AStart()�̑O�ɌĂ΂��
        _generator.LastInstantiatedPrefab.Where(instance => instance != null).Subscribe(instance =>
        {
            // Waypoint�𐶐��������Generator�R���|�[�l���g�������������Ȃ��Ƃ����Ȃ�
            // ���ԓI���������Ă���̂ŌĂяo�����ɒ���
            List<Vector3> list = _waypointManager.GetWaypointListWithWaypointType(WaypointType.Exit);
            int r = Random.Range(0, list.Count);
            instance.transform.position = list[r];

            // UI�ɏ��������n��
            // �\��������UI�̎擾
            // �A�C�R��,�̗͂̏����l���Z�b�g
            // �̗͂��ϓ����邽�т�UI���X�V
            // ����̃^�C�~���O�ő䎌

            ActorStatusUI actorStatusUI = _actorStatusUIManager.GetUnUsedUI();

            //Sprite icon = actorRxMediator.ActorStatusSO.Icon;
            //int hp = actorRxMediator.CurrentHp.Value;
            //actorStatusUI.Init(icon, hp);
            ActorStatusSO status = instance.GetComponent<Actor>().ActorStatus;
            actorStatusUI.Init(status.Icon, status.MaxHp);

            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            currentHp.Subscribe(i => actorStatusUI.SetHp(i)).AddTo(instance);

            //actorRxMediator.CurrentHp.Subscribe(i => actorStatusUI.SetHp(i)).AddTo(instance);
        });
    }
}
