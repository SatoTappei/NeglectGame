using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// Generator�Ő��������I�u�W�F�N�g�𐶐������^�C�~���O��
/// �Q�Ƃ������R���|�[�l���g���g���ď������������肷��
/// </summary>
public class GenerateObservePresenter : MonoBehaviour
{
    [SerializeField] Generator _generator;
    [Header("�Q�Ƃ��������������R���|�[�l���g�ւ̎Q��")]
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] ActorStatusUIManager _actorStatusUIManager;
    [SerializeField] ActorMonitor _actorMonitor;
    [SerializeField] GenerateControl _generateControl;

    void Awake()
    {
        // Awake()��Enabled()�̌�AStart()�̑O�ɌĂ΂��
        _generator.LastInstantiatedPrefab.Where(instance => instance != null).Subscribe(instance =>
        {
            // Waypoint�𐶐��������Generator�R���|�[�l���g�������������Ȃ��Ƃ����Ȃ�
            // ���ԓI���������Ă���̂ŌĂяo�����ɒ���
            //List<Vector3> list = _waypointManager.GetWaypointListWithWaypointType(WaypointType.Exit);
            //int r = Random.Range(0, list.Count);
            //instance.transform.position = list[r];

            _waypointManager.SetRandomWaypoint(instance, WaypointType.Exit);

            // TODO:�{���̎Q�Ǝ擾���K�萔�𒴂���UI��\�������悤�Ƃ���ƃG���[
            ActorStatusHolder statusHolder = instance.GetComponent<ActorStatusHolder>();
            ActorStatusUI statusUI = _actorStatusUIManager.GetNewActiveUI(statusHolder.Icon, statusHolder.MaxHp);

            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            System.IDisposable disposable = currentHp.Subscribe(i => statusUI.SetHp(i)).AddTo(instance);

            var currentState = instance.GetComponent<ActorStateMachine>().CurrentState;
            currentState.Where(state => state.Type == StateType.Goal || state.Type == StateType.Dead)
                .Subscribe(state =>
                {
                    disposable.Dispose();
                    statusUI.Release();

                    _actorMonitor.DetectGoalOrDeadState(state.Type);
                    _generateControl.Remove();

                }).AddTo(instance);

           _generateControl.Add();
        });
    }
}
