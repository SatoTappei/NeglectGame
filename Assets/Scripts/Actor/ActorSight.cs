using UnityEngine;
using UniRx;

/// <summary>
/// �L�����N�^�[�̎��E�𐧌䂷��R���|�[�l���g
/// </summary>
public class ActorSight : MonoBehaviour
{
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
    [Header("���E�̍X�V�Ԋu")]
    [SerializeField] float _updateDuration = 0.25f;
    [Header("���E�̃p�����[�^��ݒ�")]
    [SerializeField] float _sightRange = 5.0f;
    [SerializeField] float _sightAngle = 60.0f;

    Collider[] _results = new Collider[ResultsLength];
    SightableObject _currentInSightObject;

    internal SightableObject CurrentInSightObject => _currentInSightObject;

    public void StartInSight() => InvokeRepeating(nameof(InSightObject), 0, _updateDuration);
    public void StopInSight() { /* ���E�̍X�V���~�߂鏈�� */ }
    internal bool IsFindInSight() => _currentInSightObject != null;

    /// <summary>�����̃I�u�W�F�N�g���������ꍇ�͍ŏ���1���Ԃ�</summary>
    void InSightObject()
    {
        Physics.OverlapSphereNonAlloc(transform.position, _sightRange, _results, _sightableLayer);

        foreach (Collider col in _results)
        {
            // ����I�ɌĂяo���Ă���̂ŉ������E�ɂȂ��ꍇ��break����
            if (!col) break;

            Vector3 modelForward = _actorModel.forward;
            Vector3 treasurePos = col.gameObject.transform.position;
            Vector3 treasureDir = (treasurePos - _actorModel.position).normalized;

            float distance = Vector3.Distance(treasurePos, _actorModel.position);
            float angle = Vector3.Angle(modelForward, treasureDir);

            // �Ώۂ܂�Ray���΂��ăq�b�g���Ȃ������� �̔��肾�ƑΏۂ������ȏ�ǂɖ��܂��Ă����false���Ԃ�
            Vector3 rayOrigin = _actorModel.transform.position;
            rayOrigin.y += ActorModelHeight;
            bool dontHitObstacle = !Physics.Raycast(rayOrigin, treasureDir, distance, _sightBlockableLayer);

            if(distance <= _sightRange && angle <= _sightAngle && dontHitObstacle)
            {
                _currentInSightObject = col.gameObject.GetComponent<SightableObject>();
            }
        }
    }
}
