using UniRx;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] PauseControl _pauseControl;

    void Awake()
    {
        // Awake()��Enabled()�̌�AStart()�̑O�ɌĂ΂��
        _generator.LastInstantiatedPrefab.Where(instance => instance != null).Subscribe(instance =>
        {
            // Waypoint�𐶐��������Generator�R���|�[�l���g�������������Ȃ��Ƃ����Ȃ�
            // ���ԓI���������Ă���̂ŌĂяo�����ɒ���
            _waypointManager.SetRandomWaypoint(instance, WaypointType.Exit);

            ActorStatusHolder statusHolder = instance.GetComponent<ActorStatusHolder>();
            ActorStatusUI statusUI = _actorStatusUIManager.GetNewActiveUI(statusHolder.Icon, statusHolder.MaxHp);

            IReadOnlyReactiveProperty<int> currentHp = instance.GetComponent<ActorHpControl>().CurrentHp;
            System.IDisposable disposable = currentHp.Subscribe(i => 
            {
                statusUI.SetHp(i, statusHolder.MaxHp);
            }).AddTo(instance);

            var currentState = instance.GetComponent<ActorStateMachine>().CurrentState;
            currentState.Subscribe(state => 
            {
                string line = statusHolder.GetLineWithState(state.Type);
                statusUI.PrintLine(line);
            }).AddTo(instance);
            currentState.Where(state => state.Type == StateType.Goal || state.Type == StateType.Dead).Subscribe(state =>
            {
                disposable.Dispose();
                _actorMonitor.DetectGoalOrDeadState(state.Type);

                // �䎌�\���̎��Ԃ��m�ۂ��邽�ߒx��������
                DOVirtual.DelayedCall(1.5f, () => 
                {
                    _generateControl.CountDown();
                    statusUI.Release();
                    _pauseControl.Remove(instance);
                }).SetLink(instance);
            }).AddTo(instance);

            _generateControl.CountUp();
            _pauseControl.Add(instance);
        });
    }
}
