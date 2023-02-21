using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎��E�𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorSight : MonoBehaviour
{
    /// <summary>���E�̍X�V�Ԋu�A�Ԋu���L������Ǝ��E�̃p�����[�^�ɂ���Ă͔F�����Ȃ��Ȃ�</summary>
    static readonly float UpdateInterval = 0.25f;

    /// <summary>���E�ɉf����͂̃I�u�W�F�N�g���ɉ����Đݒ肷��</summary>
    static readonly int ResultsLength = 4;
    /// <summary>���ォ��Ray���΂����߂ɃL�����N�^�[�̃��f���̍����ɉ����Đݒ肷��</summary>
    static readonly float ActorModelHeight = 1.5f;

    [Header("������Ray���΂��L�����N�^�[���f��")]
    [SerializeField] Transform _actorModel;
    [Header("�L�����N�^�[�̎��E�ɉf�郌�C���[")]
    [SerializeField] LayerMask _sightableLayer;
    [Header("�L�����N�^�[�̎��E���Օ����郌�C���[")]
    [SerializeField] LayerMask _sightBlockableLayer;
    [Header("���E�̃p�����[�^��ݒ�")]
    [SerializeField] float _sightRange = 5.0f;
    [SerializeField] float _sightAngle = 60.0f;

    Collider[] _results = new Collider[ResultsLength];
    SightableObject _currentInSightObject;

    internal SightableObject CurrentInSightObject => _currentInSightObject;

    public void StartLookInSight() => InvokeRepeating(nameof(LookInSight), 0, UpdateInterval);
    public void StopLookInSight() => CancelInvoke(nameof(LookInSight));

    void LookInSight()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _sightRange, _results, _sightableLayer);

        // �����̃I�u�W�F�N�g���������ꍇ�͍ŏ���1���Ԃ�
        foreach (Collider rangeInSide in _results)
        {
            // ����I�ɌĂяo���Ă���̂ŉ������E�ɂȂ��ꍇ��break����
            if (rangeInSide == null) break;

            Vector3 rangeInSidePos = rangeInSide.gameObject.transform.position;
            Vector3 rangeInSideDir = (rangeInSidePos - _actorModel.position).normalized;

            float distance = Vector3.Distance(rangeInSidePos, _actorModel.position);
            float angle = Vector3.Angle(_actorModel.forward, rangeInSideDir);

            // TODO:����͖��Ȃ����ARay�̔�΂����𒼂��Ɨǂ�����
            // ���f���̓��ォ�琅����Ray���΂��ď�Q���ɏՓ˂��Ȃ�������Ƃ�������Ȃ̂�
            // ���f�����Ⴂ��Q���͖��������
            
            // �Ώۂ܂�Ray���΂��ăq�b�g���Ȃ������� �̔��肾�ƑΏۂ������ȏ�ǂɖ��܂��Ă����false���Ԃ�
            Vector3 rayOrigin = _actorModel.transform.position;
            rayOrigin.y += ActorModelHeight;
            bool dontBlocked = !Physics.Raycast(rayOrigin, rangeInSideDir, distance, _sightBlockableLayer);

            UnityEngine.Debug.DrawRay(rayOrigin, rangeInSideDir * distance, Color.red, 0.1f, false);

            if(distance <= _sightRange && angle <= _sightAngle && dontBlocked)
            {
                _currentInSightObject = rangeInSide.gameObject.GetComponent<SightableObject>();
                break;
            }
        }
    }
}