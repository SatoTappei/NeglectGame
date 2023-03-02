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

            // ���݂�Actor��SO���������Ă��邪�AActorStatus�Ƃ̒����������Ă������Ɏ�������
            // �������邱�Ƃ�Actor��SO�ւ̃v���p�e�B������
            // Manager��UI�ǂ��������삵�Ă���̂ł���ܗǂ��Ȃ��H

            // �������̓L�����N�^�[������̎擾�AUI���͂ǂ����痈�����m��Ȃ��Ă悢
            ActorStatusSO status = instance.GetComponent<Actor>().ActorStatus;
            // ��������UI���ւ̒l��n���AUI���͂ǂ����痈�����m��Ȃ��Ă悢
            ActorStatusUI statusUI = _actorStatusUIManager.GetNewActiveUI(status.Icon, status.MaxHp);

            var state = instance.GetComponent<ActorStateMachine>().CurrentState;

            // HPControl�̒l��UI�Ɋ��蓖�Ă�A��������݂���m��Ȃ��Ă悢
            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            System.IDisposable disposable = currentHp.Subscribe(i => statusUI.SetHp(i)).AddTo(instance);

            state.Where(s => s.Type == StateType.Goal || s.Type == StateType.Dead).Subscribe(_ => 
            {
                disposable.Dispose();
                statusUI.Release();
            }).AddTo(instance);

            //currentHp.Where(i => i <= 0).Skip(1).Subscribe(_ => statusUI.Release()).AddTo(instance);

            // ���݂́��̃T�u�X�N����������Ă��Ȃ��̂ł������ȋ����ɂȂ�
            // �ǂ��ɂ��L�������̔C�ӂ̃^�C�~���O�ŏ�����������悤�ɂ���
        });
    }
}
